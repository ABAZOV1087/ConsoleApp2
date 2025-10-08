/* 
Вопрос 1. Декомпозиция

using System.Diagnostics;

1.Какие существительные в задаче?

Это будущие классы/сущности:
Book(Книга)
Library(Библиотека)
Cart(Корзина)

вопрос 2.Какие глаголы в задаче?SSsSSSS

Это будущие методы/операции:
AddBook() – добавить книгу
RemoveBook() – удалить книгу
FindBookByTitle(), FindBookByAuthor(), FindBookByGenre() – поиск
SortBooksByTitle(), SortBooksByYear() – сортировка
GetMostExpensiveBook(), GetCheapestBook() – найти самую дорогую/дешёвую
GroupBooksByAuthor() – сгруппировать по авторам
ImportBooksBatch() – пакетный импорт книг
AddToCart() – добавить книгу в корзину
GetCartTotalPrice() – общая стоимость корзины


вопрос 3. Какие данные всегда вместе?
У книги: Id + Title + Author + Genre + Year + Price
всё это идёт в Book.
У корзины: список книг и итоговая сумма
всё это в Cart.


вопрос 4. Что может существовать отдельно?
Книга существует отдельно (даже без корзины).
Библиотека может быть без пользователей, просто хранить книги.
Корзина отдельно от библиотеки (каждый пользователь может иметь свою).


вопрос 5. Что повторяется?
Проверка корректности данных (можно сделать отдельный метод ValidateBook()).
Вывод информации о книге (метод ToString() в классе Book). 
*/

using System;
using System.Collections.Generic;
using System.Linq;

public enum Genre
{
    Fiction,
    Science,
    Fantasy,
    Mystery,
    Biography,
    History
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public Genre Genre { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Название: {Title}, Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
    }
}

public class ShoppingCart
{
    private List<Book> books;

    public ShoppingCart()
    {
        books = new List<Book>();
    }

    public void AddToCart(Book book)
    {
        books.Add(book);
        Console.WriteLine($"Книга '{book.Title}' добавлена в корзину");
    }

    public void RemoveFromCart(int bookId)
    {
        var book = books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine($"Книга '{book.Title}' удалена из корзины");
        }
        else
        {
            Console.WriteLine("Книга с указанным ID не найдена в корзине");
        }
    }

    public decimal CalculateTotalPrice()
    {
        return books.Sum(book => book.Price);
    }

    public void DisplayCart()
    {
        if (!books.Any())
        {
            Console.WriteLine("Корзина пуста");
            return;
        }

        Console.WriteLine("\n=== КОРЗИНА ===");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
        Console.WriteLine($"Общая стоимость: {CalculateTotalPrice():C}");
    }

    public void ClearCart()
    {
        books.Clear();
    }
}

public class Library
{
    private List<Book> books;
    private int nextId;

    public Library()
    {
        books = new List<Book>();
        nextId = 1;
        InitializeTestData();
    }

    private void InitializeTestData()
    {
        AddBook("Война и мир", "Лев Толстой", Genre.Fiction, 1869, 1200m);
        AddBook("1984", "Джордж Оруэлл", Genre.Fiction, 1949, 800m);
        AddBook("Краткая история времени", "Стивен Хокинг", Genre.Science, 1988, 950m);
        AddBook("Властелин колец", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 1500m);
        AddBook("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Mystery, 1934, 700m);
    }

    public void AddBook(string title, string author, Genre genre, int year, decimal price)
    {
        if (!Validator.ValidateBookData(title, author, year, price))
        {
            Console.WriteLine("Ошибка: Некорректные данные книги");
            return;
        }

        var book = new Book
        {
            Id = nextId++,
            Title = title.Trim(),
            Author = author.Trim(),
            Genre = genre,
            Year = year,
            Price = price
        };

        books.Add(book);
        Console.WriteLine($"Книга '{title}' успешно добавлена с ID: {book.Id}");
    }

    public void RemoveBook(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine($"Книга '{book.Title}' удалена из библиотеки");
        }
        else
        {
            Console.WriteLine("Книга с указанным ID не найдена");
        }
    }

    public List<Book> FindBooksByTitle(string title)
    {
        return books.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
    }

    public List<Book> FindBooksByAuthor(string author)
    {
        return books.Where(b => b.Author.ToLower().Contains(author.ToLower())).ToList();
    }

    public List<Book> FindBooksByGenre(Genre genre)
    {
        return books.Where(b => b.Genre == genre).ToList();
    }

    public List<Book> SortBooksByTitle()
    {
        return books.OrderBy(b => b.Title).ToList();
    }

    public List<Book> SortBooksByYear()
    {
        return books.OrderBy(b => b.Year).ToList();
    }

    public Book? GetMostExpensiveBook()
    {
        return books.OrderByDescending(b => b.Price).FirstOrDefault();
    }

    public Book? GetCheapestBook()
    {
        return books.OrderBy(b => b.Price).FirstOrDefault();
    }

    public void GroupBooksByAuthors()
    {
        var groupedBooks = books.GroupBy(b => b.Author)
        .OrderBy(g => g.Key);

        Console.WriteLine("\n=== КНИГИ ПО АВТОРАМ ===");
        foreach (var group in groupedBooks)
        {
            Console.WriteLine($"Автор: {group.Key}, Количество книг: {group.Count()}");
            foreach (var book in group)
            {
                Console.WriteLine($" - {book.Title} ({book.Year})");
            }
        }
    }

    public void ImportBooks(string booksData)
    {
        var lines = booksData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int importedCount = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (parts.Length == 5)
            {
                try
                {
                    var title = parts[0].Trim();
                    var author = parts[1].Trim();
                    var genre = ParseGenre(parts[2].Trim());
                    var year = int.Parse(parts[3].Trim());
                    var price = decimal.Parse(parts[4].Trim());

                    if (Validator.ValidateBookData(title, author, year, price))
                    {
                        AddBook(title, author, genre, year, price);
                        importedCount++;
                    }
                }
                catch
                {
                    Console.WriteLine($"Ошибка при обработке строки: {line}");
                }
            }
        }

        Console.WriteLine($"Импортировано книг: {importedCount}");
    }

    private Genre ParseGenre(string genreStr)
    {
        if (Enum.TryParse(genreStr, true, out Genre genre))
        {
            return genre;
        }
        return Genre.Fiction; 
    }

    public void DisplayAllBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Библиотека пуста");
            return;
        }

        Console.WriteLine("\n=== ВСЕ КНИГИ В БИБЛИОТЕКЕ ===");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }

    public Book? GetBookById(int id)
    {
        return books.FirstOrDefault(b => b.Id == id);
    }

    public List<Book> GetAllBooks()
    {
        return new List<Book>(books);
    }
}

public static class UserInput
{
    public static string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();
        while (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ошибка: Поле не может быть пустым");
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
        }
        return input;
    }

    public static int GetIntInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Ошибка: Введите корректное положительное число");
        }
    }

    public static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= 0)
            {
                return result;
            }
            Console.WriteLine("Ошибка: Введите корректную неотрицательную цену");
        }
    }

    public static Genre GetGenreInput(string prompt)
    {
        Console.WriteLine(prompt);
        Console.WriteLine("Доступные жанры:");
        var genres = Enum.GetValues(typeof(Genre));
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
        }

        while (true)
        {
            Console.Write("Выберите номер жанра: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= genres.Length)
            {
                return (Genre)(choice - 1);
            }
            Console.WriteLine("Ошибка: Введите корректный номер жанра");
        }
    }
}

public static class Validator
{
    public static bool ValidateBookData(string title, string author, int year, decimal price)
    {
        return !string.IsNullOrWhiteSpace(title) &&
        !string.IsNullOrWhiteSpace(author) &&
        ValidateYear(year) &&
        ValidatePrice(price);
    }

    public static bool ValidatePrice(decimal price)
    {
        return price >= 0;
    }

    public static bool ValidateYear(int year)
    {
        return year > 0 && year <= DateTime.Now.Year;
    }
}

class Program
{
    private static Library library = new Library();
    private static ShoppingCart cart = new ShoppingCart();

    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА УЧЕТА БИБЛИОТЕКИ ===");
        ShowMainMenu();
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу");
            Console.WriteLine("3. Найти книги");
            Console.WriteLine("4. Сортировать книги");
            Console.WriteLine("5. Самая дорогая/дешевая книга");
            Console.WriteLine("6. Группировка по авторам");
            Console.WriteLine("7. Показать все книги");
            Console.WriteLine("8. Пакетный импорт книг");
            Console.WriteLine("9. Корзина");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBookMenu();
                    break;
                case "2":
                    RemoveBookMenu();
                    break;
                case "3":
                    FindBooksMenu();
                    break;
                case "4":
                    SortBooksMenu();
                    break;
                case "5":
                    ShowPriceExtremes();
                    break;
                case "6":
                    library.GroupBooksByAuthors();
                    break;
                case "7":
                    library.DisplayAllBooks();
                    break;
                case "8":
                    ImportBooksMenu();
                    break;
                case "9":
                    ShoppingCartMenu();
                    break;
                case "0":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

