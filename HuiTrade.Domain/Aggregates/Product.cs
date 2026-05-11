using System;
using System.Collections.Generic;
using System.Text;


namespace HuiTrade.Domain.Aggregates
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid SellerId { get; private set; }
        public int CategoryId { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;

        public decimal Price { get; private set; }

        public string CurrencySymbol { get; private set; } = "CNY";

        private readonly List<string> _images = new();
        public IReadOnlyCollection<string> Images => _images;

        public ProductStatus Status { get; private set; }

        public int ViewCount { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Product() { }

        public static Product Create(Guid sellerId, int categoryId, string title, string description, decimal price, List<string> images)
        {
            if (images.Any(string.IsNullOrWhiteSpace)) throw new ArgumentException("图片不能为空");
            if (price <= 0) throw new ArgumentException("价格必须大于0");
            if (images == null || images.Count == 0) throw new ArgumentException("至少上传一张照片");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("标题不能为空");
            if (title.Length > 300) throw new ArgumentException("标题过长");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("描述不能为空");


            var product = new Product
            {
                Id = Guid.NewGuid(),
                SellerId = sellerId,
                CategoryId = categoryId,
                Title = title,
                Description = description,
                Price = price,
                Status = ProductStatus.Available,
                ViewCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            product._images.AddRange(images);

            return product;
        }

        public void Lock()
        {
            if (Status != ProductStatus.Available) throw new InvalidOperationException("当前商品不可售卖状态");

            this.Status = ProductStatus.Locked;
            this.UpdatedAt = DateTime.UtcNow;

        }

        public void Release()
        {

            if (Status != ProductStatus.Locked) throw new InvalidOperationException("只有锁定状态可以释放");
            Status = ProductStatus.Available;
            UpdatedAt = DateTime.UtcNow;


        }

        public void MarkAsSold()
        {
            if (Status == ProductStatus.Sold) throw new InvalidOperationException("已售商品不可操作");

            if (Status != ProductStatus.Locked) throw new InvalidOperationException("只有锁定状态的商品才能成交");

            this.Status = ProductStatus.Sold;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public void IncrementViewCount()
        {
            this.ViewCount++;
            UpdatedAt = DateTime.UtcNow;
        }



    }
}
