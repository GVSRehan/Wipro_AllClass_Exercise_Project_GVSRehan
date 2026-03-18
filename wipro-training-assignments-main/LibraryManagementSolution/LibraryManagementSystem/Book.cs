using System;

namespace LibraryManagementSystem;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsBorrowed { get; private set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsBorrowed = false;
    }

    public void Borrow()
    {
        if (IsBorrowed)
            throw new InvalidOperationException("Book is already borrowed.");

        IsBorrowed = true;
    }

    public void Return()
    {
        if (!IsBorrowed)
            throw new InvalidOperationException("Book is not borrowed.");

        IsBorrowed = false;
    }
}