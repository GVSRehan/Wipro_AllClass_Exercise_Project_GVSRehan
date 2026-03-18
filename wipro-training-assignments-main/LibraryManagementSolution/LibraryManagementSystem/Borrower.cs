using System;
using System.Collections.Generic;

namespace LibraryManagementSystem;

public class Borrower
{
    public string Name { get; set; }
    public string LibraryCardNumber { get; set; }
    public List<Book> BorrowedBooks { get; private set; }

    public Borrower(string name, string libraryCardNumber)
    {
        Name = name;
        LibraryCardNumber = libraryCardNumber;
        BorrowedBooks = new List<Book>();
    }

    public void BorrowBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        book.Borrow();
        BorrowedBooks.Add(book);
    }

    public void ReturnBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        if (!BorrowedBooks.Contains(book))
            throw new InvalidOperationException("This borrower did not borrow this book.");

        book.Return();
        BorrowedBooks.Remove(book);
    }
}