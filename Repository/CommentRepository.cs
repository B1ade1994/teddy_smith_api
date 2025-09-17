using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using teddy_smith_api.Data;
using teddy_smith_api.Interfaces;
using teddy_smith_api.Models;

namespace teddy_smith_api.Repository
{
  public class CommentRepository : ICommentRepository
  {
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
      await _context.Comments.AddAsync(comment);
      await _context.SaveChangesAsync();
      return comment;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
      return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
      return await _context.Comments.FindAsync(id);
    }
  }
}