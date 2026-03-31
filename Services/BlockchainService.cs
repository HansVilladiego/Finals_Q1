using System.Security.Cryptography;
using System.Text;
using Finals_Q1.Models;

namespace Finals_Q1.Services;

public static class BlockchainService
{
    public static string ComputeHash(TodoItem item)
    {
        var raw = $"{item.Id}{item.Title}{item.IsCompleted}{item.PreviousHash}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static bool VerifyChain(List<TodoItem> todos)
    {
        for (int i = 0; i < todos.Count; i++)
        {
            var item = todos[i];

            // Recalculate expected hash
            var expectedHash = ComputeHash(item);
            if (item.Hash != expectedHash)
                return false;

            // Check chain linkage (skip genesis block)
            if (i > 0 && item.PreviousHash != todos[i - 1].Hash)
                return false;
        }
        return true;
    }
}