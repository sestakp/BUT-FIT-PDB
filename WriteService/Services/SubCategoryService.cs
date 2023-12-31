﻿using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Category;
using Common.RabbitMQ.Messages.Customer;
using Common.RabbitMQ.Messages.Products;
using Common.RabbitMQ.Messages.SubCategory;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.DTOs.SubCategory;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class SubCategoryService
{

    private readonly ShopDbContext _context;
    private readonly RabbitMQProducer _producer;
    private readonly ILogger<CustomerService> _logger;

    static string NormalizeString(string input)
    {
        // Převod na malá písmena
        string lowercase = input.ToLower();

        // Nahrazení mezer pomlčkou
        string noSpaces = Regex.Replace(lowercase, @"\s", "-");

        // Odstranění speciálních znaků
        string normalized = RemoveDiacritics(noSpaces);

        return normalized;
    }

    static string RemoveDiacritics(string text)
    {
        string normalized = text.Normalize(NormalizationForm.FormD);
        StringBuilder result = new StringBuilder();

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                result.Append(c);
        }

        return result.ToString();
    }

    public SubCategoryService(ShopDbContext context, RabbitMQProducer producer, ILogger<CustomerService> logger)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
    }

    public async Task<ProductSubCategoryEntity> CreateAsync(CreateSubCategoryDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == dto.CategoryId);

            if (category == null)
            {
                throw new EntityNotFoundException(dto.CategoryId);
            }


            var entity = new ProductSubCategoryEntity()
            {
                Description = dto.Description,
                Name = dto.Name,
                Category = category,
                NormalizedName = NormalizeString(dto.Name)

            };

            _context.Add(entity);

            category.SubCategories.Add(entity);
            _context.Update(category);

            await _context.SaveChangesAsync();

            var message = new CreateSubCategoryMessage()
            {
                Id = entity.Id,
                Description = dto.Description,
                Name = dto.Name,
                CategoryId = category.Id

            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.ProductSubCategory, message);

            await transaction.CommitAsync();

            return entity;
        }
    }

    public async Task<ProductSubCategoryEntity> UpdateAsync(long subCategoryId, UpdateSubCategoryDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {

            var subCategory = await _context.SubCategories.FindAsync(subCategoryId);
            if (subCategory is null)
            {
                throw new EntityNotFoundException(subCategoryId);
            }




            if (subCategory.Category.Id != dto.CategoryId)
            {
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                if (category is null)
                {
                    throw new EntityNotFoundException(dto.CategoryId);
                }

                category.SubCategories.Remove(subCategory);

                subCategory.Description = dto.Description;
                subCategory.Name = dto.Name;
                subCategory.Category = category;

                category.SubCategories.Add(subCategory);

                _context.Update(category);
            }
            else
            {
                subCategory.Description = dto.Description;
                subCategory.Name = dto.Name;
            }



            _context.Update(subCategory);

            await _context.SaveChangesAsync();

            var message = new UpdateSubCategoryMessage
            {
                Id = subCategory.Id,
                CategoryId = subCategory.Category.Id,
                Description = dto.Description,
                Name = dto.Name
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.ProductSubCategory, message);

            await transaction.CommitAsync();

            return subCategory;
        }
    }

    public async Task DeleteAsync(long subCategoryId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var subCategory = await _context.SubCategories.FirstOrDefaultAsync(c => c.Id == subCategoryId);

            if (subCategory is null)
            {
                throw new EntityNotFoundException(subCategoryId);
            }

            var products = _context.Products.Where(p => p.SubCategories.Contains(subCategory));

            foreach (var product in products)
            {
                product.SubCategories.Remove(subCategory);
                _context.Update(product);
            }



            var categories = _context.Categories.Where(p => p.SubCategories.Contains(subCategory));
            foreach (var category in categories)
            {
                category.SubCategories.Remove(subCategory);
                _context.Update(category);
            }


            _context.Remove(subCategory);
            await _context.SaveChangesAsync();

            var message = new DeleteSubCategoryMessage() { Id = subCategory.Id };

            _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.ProductSubCategory, message);

            await transaction.CommitAsync();
        }
    }
}