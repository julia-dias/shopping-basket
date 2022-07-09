namespace Application.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Services.Implementations;
    using Data.Repository.Interfaces;
    using Domain.Model;
    using Moq;
    using NUnit.Framework;

    public class ItemServiceTests
    {
        private readonly ItemService itemService;
        private readonly Mock<IItemRepository> mockItemsRepository;

        private static List<Discount> discounts;
        private static List<Item> itemsDomain;

        private const decimal SoupPrice = 0.65m;
        private const decimal BreadPrice = 0.80m;
        private const decimal MilkPrice = 1.30m;
        private const decimal ApplesPrice = 1.00m;

        public ItemServiceTests()
        {
            this.mockItemsRepository = new Mock<IItemRepository>();
            this.itemService = new ItemService(this.mockItemsRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            itemsDomain = new List<Item> {
                new Item { Id = 1, Reference = "soup", Price = SoupPrice, PriceUnit = PriceUnitEnum.Unit },
                new Item { Id = 2, Reference = "bread", Price = BreadPrice, PriceUnit = PriceUnitEnum.Unit },
                new Item { Id = 3, Reference = "milk", Price = MilkPrice, PriceUnit = PriceUnitEnum.Unit },
                new Item { Id = 4, Reference = "apples", Price = ApplesPrice, PriceUnit = PriceUnitEnum.Bag },
            };
        }

        [Test]
        public async Task AddToBasketAsync_WithNullInput_ShouldReturnNull()
        {
            //Arrange
            var itemsReference = new string[] { "carrots", "apples" };

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(null);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddToBasketAsync_WithUnexistingItemReferences_ShouldDisplayError()
        {
            //Arrange
            var itemsReference = new string[] { "carrots", "random" };

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public async Task AddToBasketAsync_WithExistingAndUnexistingItemReferences_ShouldIgnoreUnexistentAndSuccess()
        {
            //Arrange
            var itemsReference = new string[] { "carrots", "apples" };

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public async Task AddToBasketAsync_WithItemsNoDiscount_ShouldReturnTotal()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "bread", "milk", "milk" };
            var expectedSubTotal = SoupPrice + BreadPrice + (2 * MilkPrice);

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.DiscountItems.Count == 0);
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.SubTotal, result.Total);
        }

        [Test]
        public async Task AddToBasketAsync_WithDiscountByItem_ShouldReturnTotalWithDiscount()
        {
            //Arrange
            var itemsReference = new string[] { "apples", "bread" };
            var expectedSubTotal = ApplesPrice + BreadPrice;
            var expectedDiscountCount = 1;
            var expectedTotal = expectedSubTotal - 0.10m;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.AreEqual(result.DiscountItems.FirstOrDefault().ItemReference, "apples");
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, expectedTotal);
        }

        [Test]
        public async Task AddToBasketAsync_WithDiscountByMultiItem_ShouldReturnTotalWithDiscount()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "soup", "bread" };
            var expectedSubTotal = (2 * SoupPrice) + BreadPrice;
            var expectedDiscountCount = 1;
            var expectedTotal = expectedSubTotal - 0.40m;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.AreEqual(result.DiscountItems.FirstOrDefault().ItemReference, "bread");
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, expectedTotal);
        }

        [Test]
        public async Task AddToBasketAsync_WithDiscountByMultiItemMultiplesTimes_ShouldReturnTotalWithMoreDiscount()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "soup", "soup", "soup", "bread", "bread", "bread" };
            var expectedSubTotal = (4 * SoupPrice) + (3 * BreadPrice);
            var expectedDiscountCount = 1;
            var expectedTotal = expectedSubTotal - 0.80m;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.AreEqual(result.DiscountItems.FirstOrDefault().ItemReference, "bread");
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, expectedTotal);
        }

        [Test]
        public async Task AddToBasketAsync_WithDiscountByMultiItem_NoItemOffer_ShouldReturnTotal()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "soup", "milk" };
            var expectedSubTotal = (2 * SoupPrice) + MilkPrice;
            var expectedDiscountCount = 0;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, result.SubTotal);
        }

        [Test]
        public async Task AddToBasketAsync_WithDiscountByMultiItem_NoQuantityOffer_ShouldReturnTotal()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "bread" };
            var expectedSubTotal = SoupPrice + BreadPrice;
            var expectedDiscountCount = 0;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, result.SubTotal);
        }

        [Test]
        public async Task AddToBasketAsync_WithBothDiscounts_ShouldReturnTotalWithBothDiscounts()
        {
            //Arrange
            var itemsReference = new string[] { "soup", "soup", "bread", "apples" };
            var expectedSubTotal = (2 * SoupPrice) + BreadPrice + ApplesPrice;
            var expectedDiscountCount = 2;
            var expectedTotal = expectedSubTotal - 0.40m - 0.10m;

            this.SetUpMocks(itemsReference);

            //Act
            var result = await this.itemService.AddToBasketAsync(itemsReference.ToList());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DiscountItems.Count, expectedDiscountCount);
            Assert.IsNotNull(result.DiscountItems.Any(x => x.ItemReference == "apples"));
            Assert.IsNotNull(result.DiscountItems.Any(x => x.ItemReference == "bread"));
            Assert.AreEqual(result.SubTotal, expectedSubTotal);
            Assert.AreEqual(result.Total, expectedTotal);
        }

        private void SetUpMocks(string[] itemsReference)
        {
            this.SetUpDiscounts();

            this.mockItemsRepository
                .Setup(m => m.GetAllItemsAsync())
                .ReturnsAsync(itemsDomain);

            var items = itemsReference.Select(x => x).Distinct().ToArray();

            this.mockItemsRepository
                .Setup(m => m.GetDiscounts(items))
                .ReturnsAsync(discounts);
        }

        private void SetUpDiscounts()
        {
            discounts = new List<Discount>{
                new ItemDiscount {
                    Id = 1,
                    Description = "Apples have a 10% discount off their normal price",
                    ItemReference = "apples",
                    DiscountPercentage = 10,
                    IsActive = true,
                },
                new MultibuyDiscount {
                    Id = 2,
                    Description = "Buy 2 tins of soup and get a loaf of bread for half price",
                    ItemReference = "soup",
                    ItemQuantity = 2,
                    ItemOfferReference = "bread",
                    ItemOfferQuantity = 1,
                    ItemOfferDiscountPercentage = 50,
                    IsActive = true,
                },
            };
        }
    }
}