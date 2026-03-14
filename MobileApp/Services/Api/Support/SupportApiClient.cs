using System;
using MobileApp.Models.Support;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Support;

public class SupportApiClient : ApiClientBase, ISupportApiClient
{
    public SupportApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<SupportTicketDto?> CreateTicketAsync(CreateTicketRequest request) =>
        PostAsync<SupportTicketDto>("support", request);

    public Task<List<SupportTicketDto>?> GetMyTicketsAsync() =>
        GetAsync<List<SupportTicketDto>>("support/my");

    public Task CloseTicketAsync(Guid ticketId) =>
        PostAsync($"support/{ticketId}/close");

    public Task ReopenTicketAsync(Guid ticketId) =>
        PostAsync($"support/{ticketId}/reopen");

    public async Task<List<SupportTicketDto>?> GetTicketsByStatusAsync(string status = "New")
        => await GetAsync<List<SupportTicketDto>>($"support?status={status}");

    public async Task<SupportTicketDto?> AssignToMeAsync(Guid ticketId)
        => await PostAsync<SupportTicketDto>($"support/{ticketId}/assign", null);

    public async Task<SupportTicketDto?> ResolveTicketAsync(Guid ticketId, ResolveTicketRequest request)
        => await PostAsync<SupportTicketDto>($"support/{ticketId}/resolve", request);
    
    public Task<List<SupportMessageDto>?> GetMessagesAsync(Guid ticketId) =>
        GetAsync<List<SupportMessageDto>>($"support/{ticketId}/messages");

    public Task<SupportMessageDto?> SendMessageAsync(Guid ticketId, SendMessageRequest request) =>
        PostAsync<SupportMessageDto>($"support/{ticketId}/messages", request);

}