﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Polly;
using Rainfall.Common.CustomException;
using Rainfall.Common.Model.Response;
using Rainfall.Model.Rainfall;
using Rainfall.Repository.Interface;
using Rainfall.Service.Implementation.Rainfall;
using Rainfall.Service.Implementation.Rainfall.Validation;
using Rainfall.Service.Interface.Rainfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.UnitTest.Service.Rainfall
{
    public class RainfallServiceTest_ShouldNot
    {
        private readonly Mock<ILogger<RainfallService>> _loggerMock = new Mock<ILogger<RainfallService>>();
        private readonly Mock<IRainfallRepository> _repoMock = new Mock<IRainfallRepository>();

        [Fact]
        public async Task GetReadings_EmptyStationId_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = string.Empty,
                count = 3
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(new httpResponse());

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(1);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, ex.httpStatusCode);
            Assert.Equal("Invalid request", ex.error.message);
            Assert.Equal("station", ex.error.detail.FirstOrDefault().propertyName);
            Assert.Equal("Please provide stationId.", ex.error.detail.FirstOrDefault().message);
            #endregion
        }

        [Fact]
        public async Task GetReadings_CountNotValidBelow1_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = "3680",
                count = 0
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(new httpResponse());

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(1);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, ex.httpStatusCode);
            Assert.Equal("Invalid request", ex.error.message);
            Assert.Equal("count", ex.error.detail.FirstOrDefault().propertyName);
            Assert.Equal("Provided count is not in range.", ex.error.detail.FirstOrDefault().message);
            #endregion
        }

        [Fact]
        public async Task GetReadings_CountNotValidAbove100_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = "3680",
                count = 101
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(new httpResponse());

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(1);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, ex.httpStatusCode);
            Assert.Equal("Invalid request", ex.error.message);
            Assert.Equal("count", ex.error.detail.FirstOrDefault().propertyName);
            Assert.Equal("Provided count is not in range.", ex.error.detail.FirstOrDefault().message);
            #endregion
        }

        [Fact]
        public async Task GetReadings_InvalidCountStationId_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = String.Empty,
                count = 105
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(new httpResponse());

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(2);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, ex.httpStatusCode);
            Assert.Equal("Invalid request", ex.error.message);

            Assert.Equal("station", ex.error.detail[0].propertyName);
            Assert.Equal("Please provide stationId.", ex.error.detail[0].message);
            Assert.Equal("count", ex.error.detail[1].propertyName);
            Assert.Equal("Provided count is not in range.", ex.error.detail[1].message);
            #endregion
        }

        [Fact]
        public async Task GetReadings_NoRecordReturned_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = "3680",
                count = 3
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(ReturnNoRecord(param.count));

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(0);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, ex.httpStatusCode);
            Assert.Equal("No readings found for the specified stationId", ex.error.message);
            #endregion
        }

        [Fact]
        public async Task GetReadings_FailedRequest_ReturnsResponseCustomException()
        {
            #region arrange
            var exceptionType = typeof(ResponseCustomException);

            var param = new GetReadingsParam()
            {
                stationId = "3680",
                count = 3
            };

            _repoMock
                .Setup(srvc => srvc.GetReadings(It.IsAny<GetReadingsParam>()))
                .ReturnsAsync(ReturnRequestFailed(param.count));

            var service = new RainfallService(_loggerMock.Object, _repoMock.Object);
            #endregion

            #region act
            var ex = await Record.ExceptionAsync(async () =>
            {
                await service.GetReadings(param);
            }) as ResponseCustomException;
            #endregion

            #region assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            ex.error.Should().NotBeNull();
            ex.error.detail.Should().NotBeNull();
            ex.error.detail.Count.Should().Be(0);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, ex.httpStatusCode);
            Assert.Equal("Something went wrong", ex.error.message);
            #endregion
        }

        #region response
        private httpResponse ReturnNoRecord(int count = 10)
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
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.csv?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.rdf?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.ttl?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.html?_sorted&_limit=1""
                            ],
                            ""limit"": {count}
                          }},
                          ""items"": []
                        }}"
            };
        }
        private httpResponse ReturnRequestFailed(int count = 10)
        {
            return new httpResponse()
            {
                success = false,
                statusCode = System.Net.HttpStatusCode.BadRequest,
                json = @$"{{
                          ""@context"": ""http://environment.data.gov.uk/flood-monitoring/meta/context.jsonld"",
                          ""meta"": {{
                            ""publisher"": ""Environment Agency"",
                            ""licence"": ""http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/"",
                            ""documentation"": ""http://environment.data.gov.uk/flood-monitoring/doc/reference"",
                            ""version"": ""0.9"",
                            ""comment"": ""Status: Beta service"",
                            ""hasFormat"": [
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.csv?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.rdf?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.ttl?_sorted&_limit=1"",
                              ""http://environment.data.gov.uk/flood-monitoring/id/stations/3680/readings.html?_sorted&_limit=1""
                            ],
                            ""limit"": {count}
                          }},
                          ""items"": []
                        }}"
            };
        }
        #endregion
    }
}
