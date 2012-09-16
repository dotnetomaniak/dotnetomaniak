using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;
    
    public class DataLoadOptionsFixture
    {
        private readonly DataLoadOptions _loadOptions;
        
        public DataLoadOptionsFixture()
        {
            _loadOptions = new DataLoadOptions();
        }

        [Fact]
        public void LoadWith_Providing_Member_Method_As_Expression_Should_Throw_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _loadOptions.LoadWith<Category>(c => c.GetType()));
        }

        [Fact]
        public void LoadWith_Providing_Member_With_Invalid_Type_Should_Throw_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _loadOptions.LoadWith<Category>(c => c.Name));
        }

        [Fact]
        public void LoadWith_When_Association_Is_Registered_More_Than_Once_Should_Throw_InvalidOperationException()
        {
            _loadOptions.LoadWith<Category>(c => c.Stories);
            Assert.Throws<InvalidOperationException>(() => _loadOptions.LoadWith<Category>(c => c.Stories));
        }

        [Fact]
        public void IsPreloaded_Should_Return_True_When_Related_EntityCollection_Is_Registered_Using_LoadWith()
        {
            _loadOptions.LoadWith<Story>(s => s.StoryTagsInternal);
            var property = GetProperty(typeof(Story), "StoryTagsInternal");
            Assert.True(_loadOptions.IsPreloaded(property));
        }
        
        [Fact]
        public void IsPreloaded_Should_Return_True_When_Related_EntityObject_Is_Registered_Using_LoadWith()
        {
            _loadOptions.LoadWith<Story>(s=>s.User);
            var property = GetProperty(typeof(Story), "User");
            Assert.True(_loadOptions.IsPreloaded(property));
        }

        [Fact]
        public void GetPreloadedMembers_Should_Return_All_Members_Registered_With_LoadWith()
        {
            _loadOptions.LoadWith<User>(u => u.UserTagsInternal);
            _loadOptions.LoadWith<Story>(s => s.Category);
            _loadOptions.LoadWith<Story>(s => s.User);
            _loadOptions.LoadWith<Story>(s => s.StoryTagsInternal);
            _loadOptions.LoadWith<StoryVote>(v => v.User);
            _loadOptions.LoadWith<StoryMarkAsSpam>(s => s.User);
            _loadOptions.LoadWith<StoryComment>(c => c.User);
            
            var preloadedMembers = _loadOptions.GetPreloadedMembers<Story>();
            Assert.Equal(3,preloadedMembers.Length);

            preloadedMembers = _loadOptions.GetPreloadedMembers<User>();
            Assert.Equal(1, preloadedMembers.Length);
        }

        private static bool MemberFilterByName(MemberInfo objMemberInfo, Object propertyName)
        {
            // Compare the name of the member function with the filter criteria.
            return objMemberInfo.Name == propertyName.ToString();
        }

        private static MemberInfo GetProperty(Type type, string propertyName)
        {
            return type.FindMembers(MemberTypes.Property,
                                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                    MemberFilterByName,
                                    propertyName).SingleOrDefault();
        }
    }
}
