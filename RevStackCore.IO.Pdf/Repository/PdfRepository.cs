using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace RevStackCore.IO.Pdf
{
	public class PdfRepository : IIORepository, IPdfRepository
	{
		private readonly PdfDbContext _dbContext;
		public PdfRepository(PdfDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual FileEntity Add(FileEntity entity)
		{
			throw new NotImplementedException();
		}

		public virtual void Delete(FileEntity entity)
		{
			throw new NotImplementedException();
		}

		public virtual IQueryable<FileEntity> Find(Expression<Func<FileEntity, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<FileEntity> Get()
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<FileEntity> Get(IOFileEncodingType type)
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<FileEntity> Get(string path, string searchPattern, SearchOption searchOption)
		{
			throw new NotImplementedException();
		}

		public virtual FileEntity GetById(string id)
		{
			string fileName = _dbContext.GeneratePdf(id);
			if (fileName == null)
			{
				return new FileEntity();
			}
			else
			{
				return _dbContext.PdfStream(fileName);
			}
		}

		public virtual FileEntity GetById(string id, IOFileEncodingType type)
		{
			throw new NotImplementedException();
		}

		public virtual FileEntity GetById(string id, IOFileEncodingType type, string format)
		{
			throw new NotImplementedException();
		}

		public virtual FileEntity GetById(string id, IOFileEncodingType type, IODataStringFormat format)
		{
			throw new NotImplementedException();
		}

		public virtual FileEntity Update(FileEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
