using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class CardRepository : ICardRepository
{
    private readonly DbSet<Card> _cards;

    public CardRepository(FinanceContext context)
    {
        this._cards = context.Cards;
    }
    
    public void Add(Card entity)
    {
        this._cards.Add(entity);
    }

    public void Delete(Card entity)
    {
        this._cards.Remove(entity);
    }

    public void Update(Card entity)
    {
        this._cards.Update(entity);
    }
}