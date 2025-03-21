﻿using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.ApiService.Requests.Leave;

public class CreateLeaveRequest
{
    public required List<LeaveDateDto> LeaveDates { get; set; }
    public int LeaveTypeId { get; set; }
    public int EmployeeId { get; set; }
}

public class LeaveDateDto
{
    public required DateOnly Date { get; set; }

    [Range(0.5, 1.0, ErrorMessage = "LeaveDuration must be either 0.5 (half day) or 1.0 (full day).")]
    public double LeaveDuration { get; set; }
}

public class UpdateLeaveRequest
{
    public int Id { get; set; }
    public required DateOnly Date { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedUserId { get; set; }
    public int LeaveTypeId { get; set; }
    public bool IsPaid { get; set; }

    [Range(0.5, 1.0, ErrorMessage = "LeaveDuration must be either 0.5 (half day) or 1.0 (full day).")]
    public double LeaveDuration { get; set; }

    public int EmployeeId { get; set; }
}

public class LeaveListRequest : ListRequest
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
}