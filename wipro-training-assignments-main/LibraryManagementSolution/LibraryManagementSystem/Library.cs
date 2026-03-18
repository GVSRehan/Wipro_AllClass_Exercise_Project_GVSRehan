using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem;

public class Library
{
    public List<Book> Books { get; private set; }
    public List<Borrower> Borrowers { get; private set; }

    public Library()
    {
        Books = new List<Book>();
        Borrowers = new List<Borrower>();
    }

    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        Books.Add(book);
    }

    public void RegisterBorrower(Borrower borrower)
    {
        if (borrower == null)
            throw new ArgumentNullException(nameof(borrower));

        Borrowers.Add(borrower);
    }

    public void BorrowBook(string isbn, string libraryCardNumber)
    {
        var book = Books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);
        if (borrower == null)
            throw new InvalidOperationException("Borrower not found.");

        borrower.BorrowBook(book);
    }

    public void ReturnBook(string isbn, string libraryCardNumber)
    {
        var book = Books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);
        if (borrower == null)
            throw new InvalidOperationException("Borrower not found.");

        borrower.ReturnBook(book);
    }

    public List<Book> ViewBooks()
    {
        return Books;
    }

    public List<Borrower> ViewBorrowers()
    {
        return Borrowers;
    }
}