using FluentAssertions;
using Newtonsoft.Json.Linq;
using Rainfall.Common.CustomException;
using Rainfall.Model.Rainfall;
using Rainfall.Service.Implementation.Rainfall.Validation;
using Xunit.Abstractions;

namespace Rainfall.UnitTest.Controller.Rainfall
{
    public class ReadingTest_IsFieldsValid_Should
    {
        [Fact]
        public void TextField_CountValid1To100_ReturnsTrue()
        {
            for (int i = 1; i <= 100; i++)
            {
                #region arrange
                var param = new GetReadingsParam()
                {
                    stationId = "0890TH",
                    count = i,
                };
                #endregion

                #region act
                var countSpec = new CountSpecification();
                bool isCountSpecSatisfied = countSpec.IsSatisfiedBy(param);
                #endregion

                #region assert
                isCountSpecSatisfied.Should().BeTrue();
                #endregion
            }
        }

        [Fact]
        public void TextField_StationIdHasValue_ReturnsTrue()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = "0890TH",
                count = 10,
            };
            #endregion

            #region act
            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isStationSpecSatidfied.Should().BeTrue();
            #endregion
        }
    }
}