using MyGen.Data;
using MyGen.Wpf.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGen.Wpf.Main;

internal class SearchViewModelRepository : IViewModelRepository<SearchViewModel>
{
   private readonly CrudableRepository _crudableRepository;

   public SearchViewModelRepository(CrudableRepository crudableRepository)
   {
      _crudableRepository = crudableRepository;
   }

   public async Task LoadModelAsync(SearchViewModel viewModel)
   {
      await Task.Run(_crudableRepository.Load);

      List<SearchResult> result = new();
      foreach (var item in _crudableRepository.GetEntities<Data.Models.Person>())
      {
         MyGen.Shared.PersonName name = new(item.Firstname, item.Lastname);
         result.Add(new PersonSearchResult(item.Id, name));
      }

      viewModel.SetResult(result);
   }

   public Task SaveModelAsync(SearchViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }
}