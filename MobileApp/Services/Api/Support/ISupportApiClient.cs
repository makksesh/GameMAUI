using System;
using MobileApp.Models.Support;

namespace MobileApp.Services.Api.Support;

public interface ISupportApiClient
{
    Task<SupportTicketDto?> CreateTicketAsync(CreateTicketRequest request);
    Task<List<SupportTicketDto>?> GetMyTicketsAsync();
    Task CloseTicketAsync(Guid ticketId);
    Task ReopenTicketAsync(Guid ticketId);
    // Moderator
    Task<List<SupportTicketDto>?> GetTicketsByStatusAsync(string status = "New");
    Task<SupportTicketDto?> AssignToMeAsync(Guid ticketId);
    Task<SupportTicketDto?> ResolveTicketAsync(Guid ticketId, ResolveTicketRequest request);
    Task<List<SupportMessageDto>?> GetMessagesAsync(Guid ticketId);
    Task<SupportMessageDto?> SendMessageAsync(Guid ticketId, SendMessageRequest request);

}