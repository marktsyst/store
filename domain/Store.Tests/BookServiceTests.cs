using Moq;
using Xunit;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                .Returns(new[] { new Book(1, "", "", "", "", 100m) });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                .Returns(new[] { new Book(2, "", "", "", "", 100m) });

            var bookService = new BookService(bookRepositoryStub.Object);
            var validIsbn = "ISBN 12345-67890";

            var actual = bookService.GetAllByQuery(validIsbn);

            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithAuthor_CallGetAllByTitleOrAuthor()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                .Returns(new[] { new Book(1, "", "", "", "", 100m) });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                .Returns(new[] { new Book(2, "", "", "", "", 100m) });

            var bookService = new BookService(bookRepositoryStub.Object);
            var invalidIsbn = "Ritchie";

            var actual = bookService.GetAllByQuery(invalidIsbn);

            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn_NoMoq()
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();

            bookRepository.ResultOfGetAllByIsbn = new[]
            {
                new Book(idOfIsbnSearch, "", "", "", "", 100m),
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new[]
            {
                new Book(idOfAuthorSearch, "", "", "", "", 100m),
            };

            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("ISBN 12345-67890");

            Assert.Collection(books, book => Assert.Equal(idOfIsbnSearch, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByTitleOrAuthor_NoMoq()
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();

            bookRepository.ResultOfGetAllByIsbn = new[]
            {
                new Book(idOfIsbnSearch, "", "", "", "", 100m),
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new[]
            {
                new Book(idOfAuthorSearch, "", "", "", "", 100m),
            };

            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("Programming");

            Assert.Collection(books, book => Assert.Equal(idOfAuthorSearch, book.Id));
        }
    }
}
