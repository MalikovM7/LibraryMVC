using FluentValidation;
using LibraryMVC.Models.Enums;
using LibraryMVC.Services.Interface;
using LibraryMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryMVC.Controllers
{
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;
        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;
        private readonly IValidator<BorrowVM> _borrowvalidator;
        public BorrowController(IBorrowService borrowService, IBookService bookService, IReaderService readerService, IValidator<BorrowVM> borrowvalidator)
        {
            _borrowService = borrowService;
            _bookService = bookService;
            _readerService = readerService;
            _borrowvalidator = borrowvalidator;
        }
        public async Task<IActionResult> Index()
        {
            var borrows = await _borrowService.ListBorrowHistoryAsync(); // not ListCurrentlyBorrowedAsync()

            var borrowVMs = borrows.Select(b => new BorrowVM
            {
                Id = b.Id,
                BookTitle = b.Book.Title,
                ReaderName = b.Reader.FirstName + " " + b.Reader.LastName,
                BorrowDate = b.BorrowedDate,
                ReturnDueDate = b.ReturnDueDate,
                IsReturned = b.Status == BorrowStatus.Returned
            }).ToList();

            return View(borrowVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var books = await _bookService.ListBooksAsync();
            var readers = await _readerService.GetAllActiveAsync(); 

            ViewBag.Books = new SelectList(books, "Id", "Title");

            ViewBag.Readers = readers
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.FirstName + " " + r.LastName
                }).ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(BorrowVM borrowVM)
        {
            var validationResult = await _borrowvalidator.ValidateAsync(borrowVM);

            if (!validationResult.IsValid)
            {
                await FillBooksAndReaders(); // I will show this helper below
                return View(borrowVM);
            }

            try
            {
                await _borrowService.BorrowBookAsync(
                    borrowVM.BookId,
                    borrowVM.ReaderId,
                    borrowVM.BorrowDate,
                    borrowVM.ReturnDueDate
                );
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message; 

                await FillBooksAndReaders();
                return View(borrowVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            await _borrowService.ReturnBookAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillBooksAndReaders()
        {
            var books = await _bookService.ListBooksAsync();
            var readers = await _readerService.GetAllActiveAsync();

            ViewBag.Books = new SelectList(books, "Id", "Title");
            ViewBag.Readers = readers.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.FirstName + " " + r.LastName
            }).ToList();
        }


    }
}
