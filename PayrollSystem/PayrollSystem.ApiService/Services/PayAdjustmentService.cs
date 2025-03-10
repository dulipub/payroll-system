using Mapster;
using PayrollSystem.ApiService.Models;
using PayrollSystem.ApiService.Repositories;
using PayrollSystem.ApiService.Requests.PayAdjustment;
using PayrollSystem.ApiService.Responses;

namespace PayrollSystem.ApiService.Services;

public interface IPayAdjustmentService
{
    Task<PayAdjustmentResponse?> Get(int id);
    Task<bool> Insert(CreatePayAdjustmentRequest request);
    Task<bool> Update(UpdatePayAdjustmentRequest request);
    Task<ListResponse<PayAdjustmentResponse>> List(PayAdjustmentListRequest request);
    Task<bool> Delete(int id);
}

public class PayAdjustmentService(IRepository<PayAdjustment> repository) : IPayAdjustmentService
{
    public async Task<PayAdjustmentResponse?> Get(int id)
    {
        var payAdjustment = await repository.GetById(id);
        return payAdjustment.Adapt<PayAdjustmentResponse>();
    }

    public async Task<bool> Insert(CreatePayAdjustmentRequest request)
    {
        var payAdjustment = request.Adapt<PayAdjustment>();
        payAdjustment.IsActive = true;
        return await repository.Add(payAdjustment);
    }

    public async Task<bool> Update(UpdatePayAdjustmentRequest request)
    {
        var payAdjustment = request.Adapt<PayAdjustment>();
        return await repository.Update(payAdjustment);
    }

    public async Task<ListResponse<PayAdjustmentResponse>> List(PayAdjustmentListRequest request)
    {
        var payAdjustments = await repository.ListAscending(x => x.IsActive == true, x => x.Id);
        var results = payAdjustments.Adapt<List<PayAdjustmentResponse>>();
        return new ListResponse<PayAdjustmentResponse>
        {
            Results = results.Take(request.PageSize).Skip((request.PageNumber - 1) * request.PageSize).ToList(),
            TotalRecords = results.Count
        };
    }

    public async Task<bool> Delete(int id)
    {
        return await repository.SoftDelete(id);
    }
}
