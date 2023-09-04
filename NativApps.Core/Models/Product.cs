namespace NativApps.Core.Models;

/// <summary>
/// Defines a product.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the product id.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product initial quantity.
    /// </summary>
    public int InitialQuantity { get; set; }
}
