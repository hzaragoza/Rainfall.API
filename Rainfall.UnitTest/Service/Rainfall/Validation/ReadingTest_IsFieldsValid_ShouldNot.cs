using FluentAssertions;
using Rainfall.Model.Rainfall;
using Rainfall.Service.Implementation.Rainfall.Validation;

namespace Rainfall.UnitTest.Controller.Rainfall
{
    public class ReadingTest_IsFieldsValid_ShouldNot
    {
        [Fact]
        public void TextField_CountNotValidBelow1_ReturnsFalse()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = "0890TH",
                count = 0,
            };
            #endregion

            #region act
            var countSpec = new CountSpecification();
            bool isCountSpecSatisfied = countSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isCountSpecSatisfied.Should().BeFalse();
            #endregion
        }

        [Fact]
        public void TextField_CountNotValidAbove100_ReturnsFalse()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = "0890TH",
                count = 101,
            };
            #endregion

            #region act
            var countSpec = new CountSpecification();
            bool isCountSpecSatisfied = countSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isCountSpecSatisfied.Should().BeFalse();
            #endregion
        }

        [Fact]
        public void TextField_StationIdEmptyValue_ReturnsFalse()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = "",
                count = 10,
            };
            #endregion

            #region act
            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isStationSpecSatidfied.Should().BeFalse();
            #endregion
        }

        [Fact]
        public void TextField_StationIdNullValue_ReturnsFalse()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = null,
                count = 10,
            };
            #endregion

            #region act
            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isStationSpecSatidfied.Should().BeFalse();
            #endregion
        }

        [Fact]
        public void TextField_StationIdWhiteSpaceValue_ReturnsFalse()
        {
            #region arrange
            var param = new GetReadingsParam()
            {
                stationId = " ",
                count = 10,
            };
            #endregion

            #region act
            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            #endregion

            #region assert
            isStationSpecSatidfied.Should().BeFalse();
            #endregion
        }
    }
}