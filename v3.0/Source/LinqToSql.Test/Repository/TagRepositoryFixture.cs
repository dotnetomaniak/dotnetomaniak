using System;
using System.Data.Linq;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Kigg.LinqToSql.Repository;
    using Kigg.LinqToSql.DomainObjects;

    public class TagRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly TagRepository _tagRepository;
        private readonly IDomainObjectFactory _factory;

        public TagRepositoryFixture()
        {
            _factory = new DomainObjectFactory();
            _tagRepository = new TagRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new TagRepository(databaseFactory.Object));
        }

        [Fact]
        public void Add_Should_Use_Database()
        {
            database.Setup(d => d.Insert(It.IsAny<Tag>())).Verifiable();

            _tagRepository.Add(_factory.CreateTag("Dummy"));
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            Assert.Throws<ArgumentException>(() => _tagRepository.Add(_factory.CreateTag("Demo")));
        }

        [Fact]
        public void Remove_Should_Use_Database()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            database.Setup(d => d.Delete(It.IsAny<Tag>())).Verifiable();

            _tagRepository.Remove(Tags[0]);
        }

        [Fact]
        public void FindById_Should_Return_Correct_Tag()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            var id = Tags[0].Id;
            var tag = _tagRepository.FindById(id);

            Assert.Equal(id, tag.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Tag()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            var uniqueName = Tags[0].UniqueName;
            var tag = _tagRepository.FindByUniqueName(uniqueName);

            Assert.Equal(uniqueName, tag.UniqueName);
        }

        [Fact]
        public void FindByName_Should_Return_Correct_Tag()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            var name = Tags[0].Name;
            var tag = _tagRepository.FindByName(name);

            Assert.Equal(name, tag.Name);
        }

        [Fact]
        public void FindMatching_Should_Return_Correct_Tags()
        {
            Tags.AddRange(new[] {_factory.CreateTag("Demo 01") as Tag, _factory.CreateTag("Demo 02") as Tag, _factory.CreateTag("Demo 03") as Tag});

            var tags = _tagRepository.FindMatching("Demo", 10);

            Assert.Equal(3, tags.Count);
        }

        [Fact]
        public void FindByUsage_Should_Return_Top_Tags()
        {
            Tags.AddRange(
                            new[]
                                {
                                    new Tag
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Dummy 01",
                                            StoryTags = new EntitySet<StoryTag>
                                                            {
                                                                new StoryTag
                                                                    {
                                                                        Story = new Story
                                                                                    {
                                                                                        Id = Guid.NewGuid()
                                                                                    }
                                                                    }
                                                            }
                                        },
                                    new Tag
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Dummy 02",
                                            StoryTags = new EntitySet<StoryTag>
                                                            {
                                                                new StoryTag
                                                                    {
                                                                        Story = new Story
                                                                                    {
                                                                                        Id = Guid.NewGuid()
                                                                                    }
                                                                    }
                                                            }
                                        },
                                    new Tag
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Dummy 03"
                                        },
                                }
                         );

            var result = _tagRepository.FindByUsage(10);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void FindAll_Should_Return_All_Tag()
        {
            Tags.Add(_factory.CreateTag("Demo") as Tag);

            var result = _tagRepository.FindAll();

            Assert.Equal(1, result.Count);
        }
    }
}