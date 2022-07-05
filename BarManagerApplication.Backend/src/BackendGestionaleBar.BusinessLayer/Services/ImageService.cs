using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendGestionaleBar.BusinessLayer.Models;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.StorageProviders.Common;
using Microsoft.EntityFrameworkCore;
using MimeMapping;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class ImageService : IImageService
{
	private readonly IDataContext dataContext;
	private readonly IStorageProvider storageProvider;
	private readonly IMapper mapper;

	public ImageService(IDataContext dataContext, IStorageProvider storageProvider, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.storageProvider = storageProvider;
		this.mapper = mapper;
	}

	public async Task DeleteAsync(Guid id)
	{
		var dbImage = await dataContext.GetAsync<Entities.Image>(id);
		if (dbImage != null)
		{
			dataContext.Delete(dbImage);
			await dataContext.SaveAsync();
			await storageProvider.DeleteAsync(dbImage.Path);
		}
	}

	public async Task<IEnumerable<Image>> GetAsync(string path)
	{
		var query = dataContext.GetData<Entities.Image>();

		if (!string.IsNullOrWhiteSpace(path))
		{
			query = query.Where(i => i.Path.Contains(path));
		}

		var images = await query.OrderBy(i => i.Path)
			.ProjectTo<Image>(mapper.ConfigurationProvider)
			.ToListAsync();

		return images;
	}

	public async Task<ImageStream> GetAsync(Guid id)
	{
		var image = await dataContext.GetAsync<Entities.Image>(id);
		if (image != null)
		{
			Stream stream = await storageProvider.ReadAsync(image.Path);
			string contentType = MimeUtility.GetMimeMapping(image.Path);
			return new ImageStream(stream, contentType);
		}

		return null;
	}

	public async Task<Image> UploadAsync(StreamFileContent content)
	{
		string path = GetFullPath(content.FileName);

		var dbImage = new Entities.Image
		{
			Path = path,
			Length = content.Length
		};

		dataContext.Insert(dbImage);
		await dataContext.SaveAsync();
		await storageProvider.SaveAsync(path, content.Content);

		return mapper.Map<Image>(dbImage);
	}

	private static string GetFullPath(string fileName)
	{
		string date = $"{DateTime.UtcNow.Day}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Year}";
		return Path.Combine(date, fileName);
	}
}