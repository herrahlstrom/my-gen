using MyGen.Model;
using MyGen.Wpf.Shared;
using System.Linq;

namespace MyGen.Wpf.Main;

internal class SearchViewModelRepository : IViewModelRepository<SearchViewModel>
{
   private readonly IModelRepository _modelRepository;

   public SearchViewModelRepository(IModelRepository modelRepository)
   {
      _modelRepository = modelRepository;
   }

   public void LoadModel(SearchViewModel viewModel)
   {
      viewModel.SetResult(from person in _modelRepository.GetPersons()
                          select new PersonSearchResult(person.Id, person.Name));
   }

   public void SaveModel(SearchViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }
}