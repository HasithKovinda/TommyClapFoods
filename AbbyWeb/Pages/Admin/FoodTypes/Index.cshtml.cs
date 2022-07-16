using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abby.DataAccess.Repository.IRepository;
using Abby.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IEnumerable<FoodType> FoodTypes { get; set; }

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    

    public void OnGet()
    {
        FoodTypes = _unitOfWork.FoodType.GetAll();
    }
}
