using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1.CategoryServices
{
    public interface ICategoryService
    {
        Task GetAndSaveCategoryAsync();
    }
}
