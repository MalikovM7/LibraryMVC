using FluentValidation;
using LibraryMVC.FluentValidation;
using LibraryMVC.Models;
using LibraryMVC.Services.Implementation;
using LibraryMVC.Services.Interface;
using LibraryMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.WebSockets;

namespace LibraryMVC.Controllers
{
    public class ReaderController : Controller
    {
        private readonly IReaderService _readerService;
        private readonly IValidator<ReaderVM> _readervalidator;
        public ReaderController (IReaderService readerService, IValidator<ReaderVM> readervalidator)
        {
            _readerService = readerService;
            _readervalidator = readervalidator;
        }
        public async Task<IActionResult> Index()
        {
            var readers = await _readerService.GetAllAsync();
            var readersVM = readers.Select(r => new ReaderVM
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                BirthDate = r.BirthDate,
                RegistrationDate = r.RegistrationDate,
                Status = r.Status

            }).ToList();
            return View(readersVM);
        }

        public async Task<IActionResult> Details(int Id)
        {

            var reader = await _readerService.GetReaderById(Id);
            if (reader == null)
            {
                return NotFound();
            }

            var readerVM = new ReaderVM
            {
                Id = reader.Id,
                FirstName = reader.FirstName,
                LastName = reader.LastName,
                BirthDate = reader.BirthDate,
                RegistrationDate = reader.RegistrationDate,
                Status = reader.Status
               

            };

            return View(readerVM);
        }



        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {

            var readers = await _readerService.GetAllAsync();
            ViewBag.Categories = new SelectList(readers, "Id", "Title");
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateAsync(ReaderVM viewModel)
        {
            var validation = await _readervalidator.ValidateAsync(viewModel);

            if (!validation.IsValid)
            {

                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(viewModel);
            }

            var reader = new Reader
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                BirthDate = viewModel.BirthDate,
                RegistrationDate = viewModel.RegistrationDate,
                Status = viewModel.Status

            };

            await _readerService.CreateAsync(reader);

            viewModel.SuccessMessage = "Your message has been sent successfully!";
            ModelState.Clear();



            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> EditAsync(int Id)
        {
            var reader = await _readerService.GetReaderById(Id);
            if (reader == null)
            {
                return NotFound();
            }

            var viewModel = new ReaderVM
            {
                Id= reader.Id,
                FirstName = reader.FirstName,
                LastName = reader.LastName,
                BirthDate = reader.BirthDate,
                RegistrationDate = reader.RegistrationDate,
                Status = reader.Status
            };

            var readers = await _readerService.GetAllAsync();
            ViewBag.Categories = new SelectList(readers, "Id", "Title");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(ReaderVM viewModel)
        {
            var validation = await _readervalidator.ValidateAsync(viewModel);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var books = await _readerService.GetAllAsync();
                ViewBag.Categories = new SelectList(books, "Id", "Title");

                return View(viewModel);
            }

            var updatedReader= new Reader
            {
                Id = viewModel.Id,
                FirstName= viewModel.FirstName,
                LastName= viewModel.LastName,
                BirthDate= viewModel.BirthDate,
                RegistrationDate= viewModel.RegistrationDate,
                Status = viewModel.Status
            };

            var result = await _readerService.EditReader(viewModel.Id, updatedReader);
            if (result == null)
            {
                return NotFound();
            }

            viewModel.SuccessMessage = "Oxucu uğurla redaktə edildi!";
            ModelState.Clear();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var reader = await _readerService.GetReaderById(Id);
            if (reader == null)
            {
                return NotFound();
            }

            var readerVM = new ReaderVM
            {
                Id=reader.Id,
                FirstName=reader.FirstName,
                LastName=reader.LastName,
                BirthDate=reader.BirthDate,
                RegistrationDate=reader.RegistrationDate,
                Status = reader.Status
            };

            return View(readerVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var result = await _readerService.DeleteAsync(Id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



    }
}
