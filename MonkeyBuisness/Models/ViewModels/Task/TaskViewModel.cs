﻿using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Task;
public class TaskViewModel
{
    public long Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "Приоритет")]
    public string Priority { get; set; }
    [Display(Name = "Готовность")]
    public string IsDone { get; set; }
    [Display(Name = "Дата создания")]
    public string Created { get; set; }
    //[Display(Name = "Deadline")]
    //public DateTime Deadline { get; set; }
    //[Display(Name = "Просрочена?")]
    //public bool IsOverdue { get; set; }
}