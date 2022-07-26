using AutoMapper;
<<<<<<< HEAD:OnyxDoc.SubscriptionService/tests/Application.UnitTests/Common/Mappings/MappingTests.cs
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using NUnit.Framework;
using System;

namespace OnyxDoc.SubscriptionService.Application.UnitTests.Common.Mappings
=======
using OnyxDoc.DocumentService.Application.Common.Mappings;
using NUnit.Framework;
using System;

namespace OnyxDoc.DocumentService.Application.UnitTests.Common.Mappings
>>>>>>> bc538261377da49ebc942042b220626ca139f0fd:OnyxDoc.DocumentService/tests/Application.UnitTests/Common/Mappings/MappingTests.cs
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        //[TestCase(typeof(Staff), typeof(StaffListDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
