function AddToCart(productId, quantity,color,choose) {
    // Retrieve the existing cart items from localStorage
    var existingCartItems = JSON.parse(localStorage.getItem('cartItems')) || [];

    // Check if the product already exists in the cart
    var existingCartItem = existingCartItems.find(function (item) {
        return item.productId === productId && item.color===color;
    });

    if (existingCartItem) {
        // Update the quantity of the existing item
        existingCartItem.quantity = parseInt(existingCartItem.quantity) + parseInt(quantity);
    } else {
        // Create a new cart item
        var newCartItem = {
            productId: productId,
            quantity: parseInt(quantity),
            color: color,
            choose: choose,
        };

        // Add the new item to the cart
        existingCartItems.push(newCartItem);
    }

    // Save the updated cart items to localStorage
    localStorage.setItem('cartItems', JSON.stringify(existingCartItems));
    location.href = '/Products/Index';

}
function GetCarts() {
    // Retrieve the cart items from localStorage
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];

    // Return the cart items
    return cartItems;
}
function RemoveCart(productId, color) {
    // Retrieve the cart from LocalStorage
    let cart = JSON.parse(localStorage.getItem('cartItems')) || [];

    // Find the index of the item to remove
    let index = cart.findIndex(item => item.productId === productId && item.color===color);

    // Remove the item from the cart if found
    if (index !== -1) {
        cart.splice(index, 1);
    }

    // Store the updated cart back to LocalStorage
    localStorage.setItem('cartItems', JSON.stringify(cart));
    location.reload();
}
function UpdateToCartOnOrder(productId, quantity, color, choose) {
    // Retrieve the existing cart items from localStorage
    var existingCartItems = JSON.parse(localStorage.getItem('cartItems'));

    // Check if the product already exists in the cart
    var existingCartItem = existingCartItems.find(function (item) {
        return item.productId === productId && item.color===color;
    });

    if (existingCartItem) {
        // Update the quantity of the existing item
        existingCartItem.quantity = parseInt(quantity);
        existingCartItem.choose = choose;
    }

    // Save the updated cart items to localStorage
    localStorage.setItem('cartItems', JSON.stringify(existingCartItems));
}
function ResetAfterSaveOrder() {
    localStorage.setItem('cartItems', "[]");
    location.reload();
}
