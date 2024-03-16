using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Rainfall.Common.CustomException;
using Rainfall.Common.Model.Response;
using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Repository.Interface;
using Rainfall.Service.Implementation.Rainfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.UnitTest.Service.Rainfall
{
    public class RainfallServiceTest_Should
    {
        private readonly Mock<ILogger<RainfallService>> _loggerMock = new Mock<ILogger<RainfallService>>();
        private readonly Mock<IRainfallRepository> _repoMock = new Mock<IRainfallRepository>();

        [Fact]
        public async Task GetReadings_ReturnsRecord_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(rainfallReadingResponse);

            var param = new GetReadingsParam()
            {
                stationId = "3680",
                count = 3
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(ReturnsRecord(param.count));

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var result = await service.GetReadings(param);
            #endregion

            #region assert
            Assert.NotNull(result);
            Assert.IsType(exceptionType, result);
            Assert.NotNull(result.readings);
            result.readings.Count.Should().Be(3);
            #endregion
        }

        #region private
        private httpResponse ReturnsRecord(int count = 10)
        {
            return new httpResponse()
            {
                success = true,
                statusCode = System.Net.HttpStatusCode.OK,
                json = @$"{{
                          ""@context"": ""http://environment.data.gov.uk/flood-monitoring/meta/context.jsonld"",
                          ""meta"": {{
                            ""publisher"": ""Environment Agency"",
                            ""licence"": ""http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/"",
                            ""documentation"": ""http://environment.data.gov.uk/flood-monitoring/doc/reference"",
                            ""version"": ""0.9"",
                            ""comment"": ""Status: Beta service"",
                            ""hasFormat"": [
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.csv?_sorted&_limit=3"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.rdf?_sorted&_limit=3"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.ttl?_sorted&_limit=3"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.html?_sorted&_limit=3""
                            ],
                            ""limit"": {count}
                          }},
                          ""items"": [
                            {{
                              ""@id"": ""http://environment.data.gov.uk/flood-monitoring/data/readings/3680-rainfall-tipping_bucket_raingauge-t-15_min-mm/2024-03-16T03-30-00Z"",
                              ""dateTime"": ""2024-03-16T03:30:00Z"",
                              ""measure"": ""http://environment.data.gov.uk/flood-monitoring/id/measures/3680-rainfall-tipping_bucket_raingauge-t-15_min-mm"",
                              ""value"": 0.0
                            }},
                            {{
                              ""@id"": ""http://environment.data.gov.uk/flood-monitoring/data/readings/3680-temperature-dry_bulb-i-15_min-deg_C/2024-03-16T03-30-00Z"",
                              ""dateTime"": ""2024-03-16T03:30:00Z"",
                              ""measure"": ""http://environment.data.gov.uk/flood-monitoring/id/measures/3680-temperature-dry_bulb-i-15_min-deg_C"",
                              ""value"": 5.44
                            }},
                            {{
                              ""@id"": ""http://environment.data.gov.uk/flood-monitoring/data/readings/3680-rainfall-tipping_bucket_raingauge-t-15_min-mm/2024-03-16T03-15-00Z"",
                              ""dateTime"": ""2024-03-16T03:15:00Z"",
                              ""measure"": ""http://environment.data.gov.uk/flood-monitoring/id/measures/3680-rainfall-tipping_bucket_raingauge-t-15_min-mm"",
                              ""value"": 0.0
                            }}
                          ]
                        }}"
            };
        }
        #endregion
    }
}
