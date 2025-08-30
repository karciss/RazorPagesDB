using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WARazorDB.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre de la tarea es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [Display(Name = "Nombre de la Tarea")]
        [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.,;:¡!¿?()-]+$", 
            ErrorMessage = "El nombre solo puede contener letras, números, espacios y caracteres básicos de puntuación")]
        [NoSpecialChars(ErrorMessage = "El nombre no puede contener caracteres especiales como @, #, $, % o &")]
        public string nombreTarea { get; set; }
        
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Vencimiento")]
        [FutureDate(ErrorMessage = "La fecha de vencimiento debe ser igual o posterior a la fecha actual")]
        public DateTime fechaVencimiento { get; set; }
        
        [Required(ErrorMessage = "El estado de la tarea es obligatorio")]
        [Display(Name = "Estado")]
        public string estado { get; set; }
        
        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo")]
        [Display(Name = "ID de Usuario")]
        public int idUsuario { get; set; }
    }
    
    //validar que la fecha sea igual o posterior a la fecha actual
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
                
            DateTime dateTime = (DateTime)value;
            return dateTime.Date >= DateTime.Today;
        }
    }
    
    //validar que el nombre de la tarea no contenga caracteres especiales
    public class NoSpecialCharsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
                
            string input = value.ToString();
            
            // Lista de caracteres especiales prohibidos
            string[] prohibitedChars = { "@", "#", "$", "%", "&", "*", "^", "~", "`", "<", ">", "{", "}", "[", "]", "|", "\\", "/" };
            
            foreach (var character in prohibitedChars)
            {
                if (input.Contains(character))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
