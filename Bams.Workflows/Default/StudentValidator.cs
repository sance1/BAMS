using Bams.Workflows.InputValidators;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using EightElements.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class StudentValidator : IStudentValidator
    {
        private IUnitOfWork _uow;
        private ITextService _text;

        public StudentValidator(IUnitOfWork uow, ITextService textService)
        {
            _uow = uow;
            _text = textService;
        }


        public async Task<List<string>> ValidateCreate(int userId, StudentDto dto)
        {
            return await ValidateStudent(userId, dto);
        }

        public async Task<List<string>> ValidateUpdate(int userId, StudentDto dto)
        {            
            return await ValidateStudent(userId, dto);
        }


        private async Task<List<string>> ValidateStudent(int userId, StudentDto dto)
        {
            var results = new List<string>();

            //TODO: move permission validation to separate service
            var admin = await _uow.AccountRepository.GetByIdAsync(userId);
            if (admin == null)
            {
                results.Add(_text.GetString("Student_popup_val_admin_account_not_found","en"));
                return results;
            }
            if (!admin.SchoolId.HasValue)
            {
                results.Add(_text.GetString("Student_popup_val_dont_have_permission_manage","en"));
                return results;
            }

            if (!admin.SchoolId.HasValue)
            {
                results.Add(_text.GetString("Student_popup_val_not_teacher_account","en"));
                return results;
            }

            var student = await _uow.UserAccountRepository.GetByUidAsync(dto.Uid);
            if (student != null)
            {
                if (student.SchoolId != admin.SchoolId)
                {
                    results.Add(_text.GetString("Student_popup_val_dont_have_permission_update","en"));
                    return results;
                }
                int activationCount = await _uow.activationCodeRepository.CountAsync(
                    c => c.UserAccountId == student.Id);
                if (activationCount > 0 && dto.Username != student.UserName)
                {
                    results.Add(_text.GetString("Student_popup_val_username_cannot_be_changed","en"));
                }
            }

            var school = await _uow.SchoolRepository.GetSingleAsync(s =>
                s.Id == admin.SchoolId.Value &&
                s.DeleteDate == null);
            if (school == null)
            {
                string message =  _text.GetString("Student_popup_val_school_not_found", "en");
                results.Add(message);
                return results;
            }

            var nameFound = await _uow.UserAccountRepository.CountAsync(u =>
                u.UserName == dto.Username && u.Uid != dto.Uid);
            if (nameFound > 0)
            {
                results.Add(_text.GetString("Student_popup_val_duplicate_username","en"));
            }

            if (string.IsNullOrEmpty(dto.Username))
            {
                results.Add(_text.GetString("Student_popup_val_username_cannot_be_empty","en"));
            }

            if (dto.Username.Length < 6)
            {
                string message = _text.GetString("Student_popup_val_username_too_short", "en");
                results.Add(message);
            }

            if (string.IsNullOrEmpty(dto.Name))
            {
                results.Add(_text.GetString("Student_popup_val_student_name_cannot_empty","en"));
            }

            if (dto.Name.Length > 100)
            {
                results.Add(_text.GetString("Student_popup_val_student_name_exceed_char","en"));
            }

            if (dto.Class.Length > 20)
            {
                results.Add(_text.GetString("Student_popup_val_class_name_exceed_char","en"));
            }

            if (dto.PhoneNumber.Length > 15)
            {
                results.Add(_text.GetString("Student_popup_val_phone_number_exceed_char","en"));
            }

            //TODO: add more email validation 
            if (!string.IsNullOrEmpty(dto.Email) && !dto.Email.Contains('@'))
            {
                results.Add(_text.GetString("Student_popup_val_invalid_email_address","en"));
            }

           if (dto.PhoneNumber.Length < 10) {
                results.Add(_text.GetString("Student_popup_val_phone_minimum_char", "en"));
            }

            if (dto.PhoneNumber.Length > 13) {
                results.Add(_text.GetString("Student_popup_val_phone_maximum_char","en"));
            }

            Regex regex = new Regex("^\\d+$");
            var onlyNumber = regex.IsMatch(dto.PhoneNumber);
            if (!onlyNumber) {
                results.Add(_text.GetString("Student_popup_val_only_number","en"));            
            }

            return results;
        }
    }
}
