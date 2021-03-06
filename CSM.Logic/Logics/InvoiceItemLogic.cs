﻿using CSM.EFCore;
using CSM.Logic.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Logic
{
    public class InvoiceItemLogic : BaseLogic
    {
        public InvoiceItemLogic(dataContext dbContext) : base(dbContext)
        {

        }

        public Task<List<InvoiceItemOrDiscount>> GetAllAsync(IsDelete status = IsDelete.Normal, bool tracking = false)
        {
            IQueryable<InvoiceItemOrDiscount> query = _DbContext.InvoiceItemOrDiscount;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            return query.ToListAsync();
        }

        public Task<List<InvoiceItemOrDiscount>> GetAsync(string invoiceId, IsDelete status = IsDelete.Normal, bool tracking = true)
        {
            IQueryable<InvoiceItemOrDiscount> query = _DbContext.InvoiceItemOrDiscount;
            if (tracking)
            {

            }
            else
            {
                query = query.AsNoTracking();
            }

            query = query.Where(h => h.IsDeleted == (int)status);

            var item = query.Where(h => h.FkInvoice == invoiceId);

            return item.ToListAsync();
        }

        public async Task<InvoiceItemOrDiscount> CreateAsync(InvoiceItemOrDiscount obj, bool saveChange = true)
        {
            var item = new InvoiceItemOrDiscount
            {
              Id = Guid.NewGuid().ToString(),
              CreationDate = DateTime.Now.ToString(),
              Creator = "Tam",
              FkInvoice = obj.FkInvoice,
              FkItemOrDiscount = obj.FkItemOrDiscount,
              IsDeleted = (int)IsDelete.Normal,
              Quantity = obj.Quantity,
              IsDiscount = obj.IsDiscount,
              Value = obj.Value
            };

            _DbContext.InvoiceItemOrDiscount.Add(item);

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return item;
        }

        public async Task<InvoiceItemOrDiscount> UpdateAsync(InvoiceItemOrDiscount obj, bool saveChange = true)
        {
            var item = await _DbContext.InvoiceItemOrDiscount.FirstOrDefaultAsync(h => h.Id == obj.Id);

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return item;
        }

        public async Task<bool> DeleteAsync(string id, bool saveChange = true)
        {
            var item = await _DbContext.InvoiceItemOrDiscount.FirstOrDefaultAsync(h => h.Id == id).ConfigureAwait(false);
            if (item == null)
            {
                return false;
            }

            // Remove cac bang lien quan

            // Remove bang chinh

            try
            {
                if (saveChange)
                {
                    await _DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }
    }
}
