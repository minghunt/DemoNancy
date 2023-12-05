using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using DemoNancy.Model;

namespace DemoNancy
{
    public class MainModule : NancyModule
    {
        public MainModule(ApplicationDbContext dbContext)
        {
            Get("/", parameters =>
            {
                var tasks = dbContext.Tasks.OrderBy(task => task.Completed).ToList();
                    var taskViewModels = tasks.Select(p => new Task
                    {
                        id = p.id,
                        Name=p.Name,
                        Description = p.Description,
                        Completed = p.Completed
                    }).ToList();
                    return View["Home.cshtml", taskViewModels];
            }
            );

            Post("/task", args =>
            {
                var task = this.Bind<Task>();

                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();

                return HttpStatusCode.Created;

                // Lấy dữ liệu từ yêu cầu POST và tạo một task mới

                // Thêm task vào cơ sở dữ liệu và lưu thay đổi

            });


            Get("/tasks/{id}", parameters =>
            {
                int taskId = parameters.id;
                var task = dbContext.Tasks.FirstOrDefault(t => t.id == taskId);

                if (task == null)
                {
                    return HttpStatusCode.NotFound;
                }

                // Trả về task dưới dạng JSON hoặc theo định dạng khác tùy ý
                return Response.AsJson(task);
            });
           
            

            Put("/tasks/{id}", parameters =>
            {
                int taskId = parameters.id;
                var task = dbContext.Tasks.FirstOrDefault(t => t.id == taskId);

                if (task == null)
                {
                    return HttpStatusCode.NotFound;
                }

                // Lấy dữ liệu từ yêu cầu PUT hoặc PATCH để cập nhật task
                var updatedTask = this.BindAndValidate<Task>();

                // Cập nhật thông tin task
                task.Name = updatedTask.Name;
                task.Description = updatedTask.Description;
                task.Completed = updatedTask.Completed;
                // Cập nhật các trường khác nếu cần

                // Lưu thay đổi vào cơ sở dữ liệu
                dbContext.SaveChanges();

                return HttpStatusCode.OK;
            });

            Delete("/tasks/{id}", parameters =>
            {
                int taskId = parameters.id;
                var task = dbContext.Tasks.FirstOrDefault(t => t.id == taskId);

                if (task == null)
                {
                    return HttpStatusCode.NotFound;
                }

                // Xóa task khỏi cơ sở dữ liệu và lưu thay đổi
                dbContext.Tasks.Remove(task);
                dbContext.SaveChanges();

                return HttpStatusCode.NoContent; // Trả về mã trạng thái 204 No Content khi xóa thành công
            });
        }

    }
}