using Nextended.Blazor.Models;
using Integrator.DataAccess.Models.Editor;
using BlazorJS;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using MudBlazor.Extensions.Components;

namespace Integrator.Frontend.WebInterface.ViewModels.Editor
{
    public interface IRichTextEditorViewModel
    {
        bool ReadOnly { get; set; }
        bool UpdateValueOnChange { get; set; }
        string EditorContent { get; set; }
        IList<UploadableFile> Files { get; }
        IEnumerable<User> Users { get; }
        IEnumerable<Tag> Tags { get; }

        Task<IEnumerable<User>> SearchUsers(string searchText);
        Task<IEnumerable<Tag>> SearchTags(string searchText);
        Task AfterUserMentionSelect(User obj);
        Task TagMentionClicked(Tag obj);
        Task UserMentionClicked(User obj);
        Task<string> OnUpload(UploadableFile arg);
    }

    public class RichTextEditorViewModel : IRichTextEditorViewModel
    {
        private readonly IJSRuntime _jsRuntime;

        public RichTextEditorViewModel(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool ReadOnly { get; set; } = false;
        public bool UpdateValueOnChange { get; set; } = false;
        public string EditorContent { get; set; }

        // Sample data for editor
        public IList<UploadableFile> Files { get; } = new List<UploadableFile>()
        {
            new()
            {
                Url = "https://mudex.azurewebsites.net/sample-data/logo.png",
                FileName = "logo.png",
                ContentType = "image/png"
            },
            new()
            {
                Url = "https://mudex.azurewebsites.net/sample-data/sample.pdf",
                FileName = "sample.pdf",
                ContentType = "application/pdf"
            },
        };

        public IEnumerable<User> Users { get; } = new List<User>()
        {
            new() { FirstName = "Florian", LastName = "Gilde", DateOfBirth = new DateTime(1983, 6,8)},
            new() { FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1974, 5,11)},
            new() { FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(1977, 12,21) },
            new() { FirstName = "Foo", LastName = "Bar", DateOfBirth = new DateTime(2000, 1,1)  },
            new() { FirstName = "Baz", LastName = "Qux", DateOfBirth = new DateTime(1990, 2,9)  },
        };

        public IEnumerable<Tag> Tags { get; } = new List<Tag>()
        {
            new() { Name = "Test"},
            new() { Name = "Whatever"},
            new() { Name = "Another"},
        };

        public Task<IEnumerable<User>> SearchUsers(string searchText)
        {
            return Task.FromResult(Users.Where(user =>
                user.FullName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)));
        }

        public Task<IEnumerable<Tag>> SearchTags(string searchText)
        {
            return Task.FromResult(Tags.Where(tag =>
                tag.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)));
        }

        public Task AfterUserMentionSelect(User obj)
        {
            Console.WriteLine(obj.FullName);
            return Task.CompletedTask;
        }

        public Task TagMentionClicked(Tag obj) =>
            _jsRuntime.AlertAsync(JsonConvert.SerializeObject(obj)).AsTask();

        public Task UserMentionClicked(User obj) =>
            _jsRuntime.AlertAsync(JsonConvert.SerializeObject(obj)).AsTask();

        public async Task<string> OnUpload(UploadableFile arg)
        {
            // W prawdziwej aplikacji tutaj byłby kod uploadujący plik
            return await DataUrl.GetDataUrlAsync(arg.Data, arg.ContentType);
        }
    }
}
