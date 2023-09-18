using BAMS.Data.Interface;
using BAMS.Models;
using EightElements.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAMS.InputValidators
{
    public class PageTextValidator
    {
        public async static Task<List<string>> ValidateCreatePageText(
            CreatePageTextDto dto, IUnitOfWork uow, ITextService textService)
        {
            var results = new List<string>();

            var keyUsage = await uow.pageTextRepository.CountAsync(a => a.Key == dto.Key);
            /*if (keyUsage > 0)
            {
                results.Add("the key has already been used.");
            }*/

            ValidatePageTextData(
                ref results,
                dto.Key, dto.Text, dto.LanguageCode,textService);

            return results;
        }

        private static void ValidatePageTextData(
            ref List<string> results,
            string key, string text, string languageCode,ITextService textService)
        {
            if (string.IsNullOrEmpty(key))
            {
                results.Add(textService.GetString("Pagetexts_popup_txt_key_cant_be_empty","en"));
            }
       
            if (string.IsNullOrEmpty(text))
            {
                results.Add(textService.GetString("Pagetexts_popup_txt_text_cannot_be_empty","en"));
            }
            if (string.IsNullOrEmpty(languageCode))
            {
                results.Add(textService.GetString("Pagetexts_popup_txt_language_code_cant_empty","en"));
            }

        }

        public async static Task<List<string>> ValidateUpdatePageText(
            int pageTextId, UpdatePageTextDto dto, IUnitOfWork uow,ITextService textService)
        {
            var results = new List<string>();
            var keyUsage = await uow.pageTextRepository.CountAsync(p => p.Key == dto.Key && p.Id != pageTextId);
           /* if (keyUsage > 0)
            {
                results.Add("the key name has already been used.");
            }*/

            ValidatePageTextData(
                ref results,
                dto.Key, dto.Text, dto.LanguageCode, textService);

            return results;
        }


    }
}
