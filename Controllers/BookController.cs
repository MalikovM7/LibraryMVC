using FluentValidation;
using LibraryMVC.Models;
using LibraryMVC.Services.Interface;
using LibraryMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LibraryMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IValidator<BookVM> _bookvalidator;

        public BookController(IBookService bookService, IValidator<BookVM> bookvalidator)
        {
            _bookService = bookService;
            _bookvalidator = bookvalidator;
        }

        public async Task<IActionResult> Index(string authorFilter, string genreFilter)
        {
            
            var books = await _bookService.ListBooksAsync(genreFilter, authorFilter);

            var bookVMs = books.Select(b => new BookVM
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                PublishDate = b.PublishDate,
                PageCount = b.PageCount,
                Status = b.Status
            }).ToList();

            return View(bookVMs);
        }


        public async Task<IActionResult> Details(int Id)
        {

            var book = await _bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Genre = book.Genre,
                Author = book.Author,
                PublishDate = book.PublishDate,
                PageCount = book.PageCount,
                Status = book.Status
              
            };

            return View(bookVM);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {

            var books = await _bookService.ListBooksAsync();
            ViewBag.Categories = new SelectList(books, "Id", "Title");
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> CreateAsync(BookVM viewModel)
        {
            var validation = await _bookvalidator.ValidateAsync(viewModel);

            if (!validation.IsValid)
            {

                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(viewModel);
            }

            var book = new Book
            {
                Author = viewModel.Author,
                Title = viewModel.Title,
                Genre = viewModel.Genre,
                PublishDate = viewModel.PublishDate,
                PageCount = viewModel.PageCount,
                Status = viewModel.Status

            };

            await _bookService.CreateAsync(book);

            viewModel.SuccessMessage = "Your message has been sent successfully!";
            ModelState.Clear();


           
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int Id)
        {
            var book = await _bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookVM
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                PageCount = book.PageCount,
                Status = book.Status
            };

            var books = await _bookService.ListBooksAsync();
            ViewBag.Categories = new SelectList(books, "Id", "Title");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(BookVM viewModel)
        {
            var validation = await _bookvalidator.ValidateAsync(viewModel);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var books = await _bookService.ListBooksAsync();
                ViewBag.Categories = new SelectList(books, "Id", "Title");

                return View(viewModel);
            }

            var updatedBook = new Book
            {
                Id = viewModel.Id,
                Author = viewModel.Author,
                Title = viewModel.Title,
                Genre = viewModel.Genre,
                PublishDate = viewModel.PublishDate,
                PageCount = viewModel.PageCount,
                Status = viewModel.Status
            };

            var result = await _bookService.EditBookAsync(viewModel.Id, updatedBook);
            if (result == null)
            {
                return NotFound();
            }

            viewModel.SuccessMessage = "Kitab uğurla redaktə edildi!";
            ModelState.Clear();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var book = await _bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                PageCount = book.PageCount,
                Status = book.Status
            };

            return View(bookVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.DeleteAsync(id);

            if (!result)
            {
                TempData["ErrorMessage"] = "Book cannot be deleted. It may have active or historical borrow records.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Book deleted successfully.";
            return RedirectToAction(nameof(Index));
        }





    }
}
