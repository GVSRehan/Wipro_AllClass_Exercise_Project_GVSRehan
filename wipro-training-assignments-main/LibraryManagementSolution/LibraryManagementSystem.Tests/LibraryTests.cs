using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryManagementSystem;
using System.Linq;

namespace LibraryManagementSystem.Tests;

[TestClass]
public class LibraryTests
{
    private Library _library = null!;

    [TestInitialize]
    public void Setup()
    {
        _library = new Library();
    }

    [TestMethod]
    public void AddBook_ShouldAddBookToLibrary()
    {
        var book = new Book("Test Book", "Author A", "123");

        _library.AddBook(book);

        Assert.AreEqual(1, _library.Books.Count);
        Assert.AreEqual("Test Book", _library.Books.First().Title);
    }

    [TestMethod]
    public void RegisterBorrower_ShouldAddBorrower()
    {
        var borrower = new Borrower("John", "CARD1");

        _library.RegisterBorrower(borrower);

        Assert.AreEqual(1, _library.Borrowers.Count);
        Assert.AreEqual("John", _library.Borrowers.First().Name);
    }

    [TestMethod]
    public void BorrowBook_ShouldMarkBookAsBorrowed()
    {
        var book = new Book("Book1", "Author1", "111");
        var borrower = new Borrower("Alice", "CARD2");

        _library.AddBook(book);
        _library.RegisterBorrower(borrower);

        _library.BorrowBook("111", "CARD2");

        Assert.IsTrue(book.IsBorrowed);
        Assert.AreEqual(1, borrower.BorrowedBooks.Count);
    }

    [TestMethod]
    public void ReturnBook_ShouldMarkBookAsAvailable()
    {
        var book = new Book("Book2", "Author2", "222");
        var borrower = new Borrower("Bob", "CARD3");

        _library.AddBook(book);
        _library.RegisterBorrower(borrower);
        _library.BorrowBook("222", "CARD3");

        _library.ReturnBook("222", "CARD3");

        Assert.IsFalse(book.IsBorrowed);
        Assert.AreEqual(0, borrower.BorrowedBooks.Count);
    }

    [TestMethod]
    public void ViewBooks_ShouldReturnAllBooks()
    {
        _library.AddBook(new Book("B1", "A1", "1"));
        _library.AddBook(new Book("B2", "A2", "2"));

        var books = _library.ViewBooks();

        Assert.AreEqual(2, books.Count);
    }

    [TestMethod]
    public void ViewBorrowers_ShouldReturnAllBorrowers()
    {
        _library.RegisterBorrower(new Borrower("User1", "C1"));
        _library.RegisterBorrower(new Borrower("User2", "C2"));

        var borrowers = _library.ViewBorrowers();

        Assert.AreEqual(2, borrowers.Count);
    }
}