using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes;

[BindProperties]
public class EditModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public FoodType FoodType { get; set; }

    public EditModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public void OnGet(int id)
    {
        FoodType = _unitOfWork.FoodType.GetFirstOrDefault(u=>u.Id==id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.FoodType.Update(FoodType);
            _unitOfWork.Save();
            TempData["success"] = "FoodType updated successfully";
            return RedirectToPage("Index");
        }
        return Page();
    }
}
