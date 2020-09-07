using System;
using System.Linq;
using System.Threading.Tasks;
using Eonet.Core;
using Eonet.Core.Models;
using Moq;
using NaturalEventsViewer.Domain.Models;
using NaturalEventsViewer.Domain.Tests.Data;
using NaturalEventsViewer.Domain.Tests.Extensions;
using NUnit.Framework;

namespace NaturalEventsViewer.Domain.Tests
{
    [TestFixture]
    public class EonetRepositoryTests
    {
        private const int MaxDaysPrior = 10;

        private Mock<IEonetApiClient> _eonetApiClientMock;
        private Mock<ICurrentTimeProvider> _currentTimeProviderMock;

        private EonetRepository _eonetRepository;

        [SetUp]
        public void SetUp()
        {
            _eonetApiClientMock = new Mock<IEonetApiClient>();
            _currentTimeProviderMock = new Mock<ICurrentTimeProvider>();

            _eonetRepository = new EonetRepository(
                _eonetApiClientMock.Object,
                _currentTimeProviderMock.Object,
                MaxDaysPrior
            );
        }

        [TestCase(EonetEventStatus.Open)]
        [TestCase(EonetEventStatus.Closed)]
        [TestCase(null)]
        public async Task GetEvents_ReturnsCorrectSetDependingOnStatus(EonetEventStatus? status)
        {
            // Arrange
            EventsResponse expectedResult;
            switch (status)
            {
                case EonetEventStatus.Open:
                    SetupOpenEventsApiResponse(); // only open events should be accessed from API in this case
                    expectedResult = EventsResponses.OpenEventsResponse;
                    break;
                case EonetEventStatus.Closed:
                    SetupClosedEventsApiResponse(); // only closed events should be accessed from API in this case
                    expectedResult = EventsResponses.ClosedEventsResponse;
                    break;
                case null:
                default:
                    SetupOpenEventsApiResponse();
                    SetupClosedEventsApiResponse();
                    expectedResult = GetBothOpenAndClosedEventsResponse();
                    break;
            }

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { Status = status });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetEvents_DaysFilter_HappyPath(int days)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var now = new DateTime(2020, 9, 7, 13, 30, 0);
            _currentTimeProviderMock.Setup(ct => ct.GetCurrentTime()).Returns(now);

            var expectedResult = GetBothOpenAndClosedEventsResponse();
            if (days != 0)
            {
                expectedResult.Events = expectedResult.Events.Where(e => e.LastDate > now.AddDays(-1 * days)).ToArray();
                expectedResult.TotalCount = expectedResult.Events.Length;
            }

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { Days = days });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCase()] // null means all
        [TestCase("PDC")]
        [TestCase("InciWeb", "LovelySource")]
        [TestCase("PDC", "InciWeb", "LovelySource")]
        public async Task GetEvents_SourcesFilter_HappyPath(params string[] sources)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse();
            if (sources != null && sources.Length > 0)
            {
                expectedResult.Events = expectedResult.Events.Where(e => e.Sources.Any(c => sources.Contains(c.Id))).ToArray();
                expectedResult.TotalCount = expectedResult.Events.Length;
            }

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { Sources = sources });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCase()] // null means all
        [TestCase("hard_wildfires")]
        [TestCase("light_wildfires", "wildfires")]
        public async Task GetEvents_CategoriesFilter_HappyPath(params string[] categories)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse();
            if (categories != null && categories.Length > 0)
            {
                expectedResult.Events = expectedResult.Events.Where(e => categories.Contains(e.Category.Id)).ToArray();
                expectedResult.TotalCount = expectedResult.Events.Length;
            }

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { Categories = categories });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCase("", "")] // null means all
        [TestCase(null, "")]
        [TestCase("California", "california")]
        [TestCase("    fIrE    ", "fire")]
        public async Task GetEvents_TitleSearchFilter_HappyPath(string titleSearch, string cleanTitleSearch)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse();
            if (!string.IsNullOrWhiteSpace(titleSearch))
            {
                expectedResult.Events = expectedResult.Events.Where(e => e.Title.ToLower().Contains(cleanTitleSearch)).ToArray();
                expectedResult.TotalCount = expectedResult.Events.Length;
            }

            expectedResult.TitleSearch = cleanTitleSearch;

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { TitleSearch = titleSearch });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCaseSource("OrderingTestConfigs")]
        public async Task GetEvents_Ordering_HappyPath(OrderingTestConfig testConfig)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse();
            expectedResult.Events = testConfig.OrderingDefinition(expectedResult.Events);

            // Act
            var actualResult = await _eonetRepository.GetEvents(new EventsRequest { Ordering = testConfig.OrderingInput });

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [Test]
        public void GetEvents_Ordering_ExceptionOnDuplicateKeys()
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var orderingWithDuplicateKey = new[] {
                new EventOrderAttribute { AttributeType = EventOrderAttributeType.Category, IsDescending = false },
                new EventOrderAttribute { AttributeType = EventOrderAttributeType.Category, IsDescending = false },
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _eonetRepository.GetEvents(new EventsRequest { Ordering = orderingWithDuplicateKey }));
            _eonetApiClientMock.VerifyAll();
        }

        [Test]
        public async Task GetCurrentlyAvailableCategories_HappyPath()
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse().Events
                .Select(e => new { e.Category.Id, e.Category.Title })
                .Distinct()
                .Select(c => new EonetCategory
                {
                    Id = c.Id,
                    Title = c.Title
                })
                .ToArray();

            // Act
            var actualResult = await _eonetRepository.GetCurrentlyAvailableCategories();

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [Test]
        public async Task GetCurrentlyAvailableSourceIds_HappyPath()
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse().Events
                .SelectMany(e => e.Sources.Select(s => s.Id))
                .Distinct()
                .ToArray();

            // Act
            var actualResult = await _eonetRepository.GetCurrentlyAvailableSourceIds();

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        [TestCase("EONET_4959")]
        [TestCase("EONET_4957")]
        [TestCase(null)]
        [TestCase("Does not exist")]
        public async Task GetSingeEvent_HappyPath(string id)
        {
            // Arrange
            SetupOpenEventsApiResponse();
            SetupClosedEventsApiResponse();

            var expectedResult = GetBothOpenAndClosedEventsResponse().Events.FirstOrDefault(e => e.Id == id);

            // Act
            var actualResult = await _eonetRepository.GetSingeEvent(id);

            // Assert
            AssertEx.PropertyValuesAreEquals(actualResult, expectedResult);
            _eonetApiClientMock.VerifyAll();
        }

        private static OrderingTestConfig[] OrderingTestConfigs => new []
        {
            new OrderingTestConfig(new [] {
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Category, IsDescending = false },
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Id, IsDescending = true },
                },
                e => e.OrderBy(p => p.Category.Title).ThenByDescending(p => p.Id).ToArray()),
            new OrderingTestConfig(new [] {
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Source, IsDescending = true },
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Category, IsDescending = false },
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Title, IsDescending = false },
                },
                e => e.OrderByDescending(p => p.Sources.Max(s => s.Id)).ThenBy(p => p.Category.Title).ThenBy(p => p.Title).ToArray()),
            new OrderingTestConfig(new [] {
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.Status, IsDescending = false },
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.ClosedDate, IsDescending = false },
                },
                e => e.OrderBy(p => p.Status).ThenBy(p => p.ClosedDate).ToArray()),
            new OrderingTestConfig(new [] {
                    new EventOrderAttribute { AttributeType = EventOrderAttributeType.LastDate, IsDescending = true }
                },
                e => e.OrderByDescending(p => p.LastDate).ToArray()),
        };

        private void SetupOpenEventsApiResponse()
        {
            _eonetApiClientMock.Setup(a => a.GetEventsAsync(It.Is<EonetEventsRequest>(e =>
                    e.Days == MaxDaysPrior
                    && (e.Status == null || e.Status == EonetEventStatus.Open)
                    && e.Limit == null
                    && e.Sources.Length == 0
                )))
                .Returns(Task.FromResult(EonetApiResponses.EonetOpenEventsResponse));
        }

        private void SetupClosedEventsApiResponse()
        {
            _eonetApiClientMock.Setup(a => a.GetEventsAsync(It.Is<EonetEventsRequest>(e =>
                    e.Days == MaxDaysPrior
                    && e.Status == EonetEventStatus.Closed
                    && e.Limit == null
                    && e.Sources.Length == 0)))
                .Returns(Task.FromResult(EonetApiResponses.EonetClosedEventsResponse));
        }

        private EventsResponse GetBothOpenAndClosedEventsResponse()
        {
            var response = EventsResponses.OpenEventsResponse;
            response.Events = response.Events.Concat(EventsResponses.ClosedEventsResponse.Events).ToArray();
            response.TotalCount = response.Events.Length;
            return response;
        }

        public class OrderingTestConfig
        {
            public EventOrderAttribute[] OrderingInput { get; set; }
            public Func<Event[], Event[]> OrderingDefinition { get; set; }

            public OrderingTestConfig(EventOrderAttribute[] orderingInput, Func<Event[], Event[]> orderingFunction)
            {
                OrderingInput = orderingInput;
                OrderingDefinition = orderingFunction;
            }
        }
    }
}
