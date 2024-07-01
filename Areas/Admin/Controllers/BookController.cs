using BookBazaar.Entities;
using BookBazaar.ViewModels;
using BookBazaar.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork<Book> _Book;
        private readonly IUnitOfWork<Author> _author;
        private readonly IUnitOfWork<Category> _category;
        private readonly IUnitOfWork<Cover> _cover;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BookController(IUnitOfWork<Book> Book, IUnitOfWork<Category> category, IUnitOfWork<Cover> cover, IUnitOfWork<Author> author, IWebHostEnvironment hostEnvironment)
        {
            _Book = Book;
            _category = category;
            _cover = cover;
            _author = author;
            _hostEnvironment = hostEnvironment;
        }

        // GET: BookController
        public async Task<ActionResult> Index()
        {
            IEnumerable<Book> Books = await _Book.Entity.GetAllAsync();
            return View(Books);
        }

        // GET: BookController/Upsert/5
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Category> categories = await _category.Entity.GetAllAsync();
            IEnumerable<Cover> covers = await _cover.Entity.GetAllAsync();
            IEnumerable<Author> authors = await _author.Entity.GetAllAsync();

            BookViewModel model = new()
            {
                Book = new(),

                Categories = categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Covers = covers.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Authors = authors.Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                // create Book
                //ViewBag.Categories = model.Categories;
                //ViewData["Covers"] = model.Covers;
                return View(model);
            }
            else
            {
                // update Book
                model.Book = await _Book.Entity.GetFirstOrDefaultAsync(m => m.Id == id);
                return View(model);
            }
        }

        // POST: BookController/Upsert/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Upsert(BookViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\Books");
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (model.Book.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, model.Book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.Book.ImageUrl = @"\images\Books\" + fileName + extension;


                    ResizeImage(file, Path.Combine(uploads, fileName + extension));
                }

                if (model.Book.Id == 0)
                {
                    await _Book.Entity.InsertAsync(model.Book);
                }
                else
                {
                    await _Book.Entity.UpdateAsync(model.Book);
                }

                await _Book.CompleteAsync();
                TempData["Success"] = "Book created successfully.";

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public void ResizeImage(IFormFile file, string path)
        {
            //using var image = Image.Load(file.OpenReadStream());
            //image.Mutate(x => x.Resize(456, 650));
            //image.Save(path);
        }
    }
}
