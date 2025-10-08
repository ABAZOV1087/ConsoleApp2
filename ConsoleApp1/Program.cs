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
