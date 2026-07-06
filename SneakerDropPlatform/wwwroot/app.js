document.addEventListener('DOMContentLoaded', () => {
    // DOM Elements
    const dropsGrid = document.getElementById('drops-grid');
    
    // Waitlist elements
    const waitlistModal = document.getElementById('waitlist-modal');
    const waitlistForm = document.getElementById('waitlist-form');
    const emailInput = document.getElementById('email-input');
    const modalSneakerName = document.getElementById('modal-sneaker-name');
    const modalDropId = document.getElementById('modal-drop-id');
    const toast = document.getElementById('toast');
    
    // Cart elements
    const cartBtn = document.getElementById('cart-btn');
    const cartBadge = document.getElementById('cart-badge');
    const cartSidebar = document.getElementById('cart-sidebar');
    const cartOverlay = document.getElementById('cart-overlay');
    const closeCartBtn = document.getElementById('close-cart');
    const cartItemsContainer = document.getElementById('cart-items');
    const cartTotalElement = document.getElementById('cart-total');
    const proceedCheckoutBtn = document.getElementById('checkout-btn');
    
    // Checkout elements
    const checkoutModal = document.getElementById('checkout-modal');
    const checkoutOverlays = document.querySelectorAll('.checkout-modal-overlay, .close-checkout-modal');
    const checkoutForm = document.getElementById('checkout-form');
    const checkoutSubmitTotal = document.getElementById('checkout-submit-total');
    const checkoutSuccess = document.getElementById('checkout-success');
    
    // General overlay elements for modal closure
    const modalOverlays = document.querySelectorAll('.modal-overlay, .close-modal');

    // State
    let cart = JSON.parse(localStorage.getItem('hype_cart')) || [];
    let dropsCache = [];

    // Init
    updateCartUI();
    fetchDrops();

    // ----------------------------------------------------
    // API & Rendering
    // ----------------------------------------------------
    async function fetchDrops() {
        try {
            const response = await fetch('/api/drops');
            if (!response.ok) throw new Error('Network response was not ok');
            dropsCache = await response.json();
            renderDrops(dropsCache);
        } catch (error) {
            console.error('Error fetching drops:', error);
            dropsGrid.innerHTML = `
                <div class="col-span-full text-center py-20 text-red-500 bg-red-500/10 rounded-2xl border border-red-500/20">
                    <i class="fa-solid fa-triangle-exclamation text-3xl mb-4"></i>
                    <p>Failed to load drops. Please try again later.</p>
                </div>
            `;
        }
    }

    function renderDrops(drops) {
        if (!drops || drops.length === 0) {
            dropsGrid.innerHTML = `<div class="col-span-full text-center py-20 text-gray-400">No active drops right now. Check back soon.</div>`;
            return;
        }

        dropsGrid.innerHTML = '';
        
        drops.forEach(drop => {
            const isLowStock = drop.totalStock > 0 && drop.totalStock <= 10;
            const isSoldOut = drop.totalStock === 0;
            const canBuy = drop.totalStock > 0;
            
            let stockBadge = `<span class="bg-white/10 text-white text-xs font-bold px-2 py-1 rounded">Stock: ${drop.totalStock}</span>`;
            if (isSoldOut) {
                stockBadge = `<span class="bg-red-500/20 text-red-500 border border-red-500/50 text-xs font-bold px-2 py-1 rounded">SOLD OUT</span>`;
            } else if (isLowStock) {
                stockBadge = `<span class="bg-red-500 text-white badge-low-stock text-xs font-bold px-2 py-1 rounded uppercase tracking-wider">Low Stock</span>`;
            }

            const dropDate = new Date(drop.dropDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });

            const btnClass = canBuy 
                ? 'add-to-cart-btn bg-neon hover:bg-white text-black font-black uppercase tracking-wider py-3 rounded-xl transition-all duration-300 shadow-[0_0_15px_rgba(204,255,0,0.2)]'
                : 'waitlist-btn bg-white/5 hover:bg-white text-white hover:text-black border border-white/10 hover:border-white font-bold uppercase tracking-wider py-3 rounded-xl transition-all duration-300';
            
            const btnText = canBuy ? '<i class="fa-solid fa-cart-plus mr-2"></i> Add to Cart' : (isSoldOut ? 'Unavailable' : 'Join Waitlist');

            const card = document.createElement('div');
            card.className = 'sneaker-card rounded-2xl flex flex-col group';
            card.innerHTML = `
                <div class="sneaker-image-container h-72 bg-white flex items-center justify-center p-8 rounded-t-2xl relative">
                    <img src="${drop.imageUrl}" alt="${drop.modelName}" class="max-w-full max-h-full object-contain filter drop-shadow-xl z-10">
                    <div class="sneaker-image-overlay"></div>
                    <div class="absolute top-4 left-4 z-20">
                        <span class="bg-black/50 backdrop-blur-md border border-white/10 text-white text-xs font-bold px-3 py-1.5 rounded-full uppercase tracking-wider">
                            ${drop.brandName}
                        </span>
                    </div>
                </div>
                <div class="p-6 flex-1 flex flex-col">
                    <div class="flex justify-between items-start mb-2">
                        <h3 class="text-2xl font-bold leading-tight">${drop.modelName}</h3>
                        <span class="text-neon font-black text-xl">$${drop.price}</span>
                    </div>
                    <div class="flex items-center gap-3 mb-6 mt-2">
                        <div class="text-gray-400 text-sm"><i class="fa-regular fa-calendar mr-2"></i>${dropDate}</div>
                        ${stockBadge}
                    </div>
                    
                    <div class="mt-auto">
                        <button class="w-full ${btnClass}"
                            data-id="${drop.dropId}" 
                            data-name="${drop.brandName} ${drop.modelName}"
                            data-price="${drop.price}"
                            data-img="${drop.imageUrl}"
                            ${isSoldOut ? 'disabled' : ''}>
                            ${btnText}
                        </button>
                    </div>
                </div>
            `;
            dropsGrid.appendChild(card);
        });

        // Event Listeners for action buttons
        document.querySelectorAll('.waitlist-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                if (btn.disabled) return;
                openWaitlistModal(btn.getAttribute('data-id'), btn.getAttribute('data-name'));
            });
        });

        document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                if (btn.disabled) return;
                addToCart({
                    id: parseInt(btn.getAttribute('data-id')),
                    name: btn.getAttribute('data-name'),
                    price: parseFloat(btn.getAttribute('data-price')),
                    imageUrl: btn.getAttribute('data-img')
                });
            });
        });
    }

    // ----------------------------------------------------
    // Cart Logic
    // ----------------------------------------------------
    function addToCart(product) {
        const existingItem = cart.find(item => item.id === product.id);
        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            cart.push({ ...product, quantity: 1 });
        }
        saveCart();
        openCart();
    }

    function updateQuantity(id, change) {
        const item = cart.find(item => item.id === id);
        if (item) {
            item.quantity += change;
            if (item.quantity <= 0) {
                cart = cart.filter(item => item.id !== id);
            }
            saveCart();
        }
    }

    function saveCart() {
        localStorage.setItem('hype_cart', JSON.stringify(cart));
        updateCartUI();
    }

    function updateCartUI() {
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        const totalPrice = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        
        // Badge
        if (totalItems > 0) {
            cartBadge.textContent = totalItems;
            cartBadge.classList.remove('hidden');
        } else {
            cartBadge.classList.add('hidden');
        }

        // Sidebar
        cartTotalElement.textContent = `$${totalPrice.toFixed(2)}`;
        checkoutSubmitTotal.textContent = `($${totalPrice.toFixed(2)})`;
        proceedCheckoutBtn.disabled = totalItems === 0;

        cartItemsContainer.innerHTML = '';
        
        if (cart.length === 0) {
            cartItemsContainer.innerHTML = `<div class="text-center py-10 text-gray-500">Your cart is empty.</div>`;
            return;
        }

        cart.forEach(item => {
            const cartRow = document.createElement('div');
            cartRow.className = 'flex gap-4 bg-[#111] p-4 rounded-xl border border-white/5 relative';
            cartRow.innerHTML = `
                <div class="w-20 h-20 bg-white rounded-lg flex items-center justify-center p-2">
                    <img src="${item.imageUrl}" alt="${item.name}" class="max-w-full max-h-full object-contain">
                </div>
                <div class="flex-1">
                    <h4 class="font-bold leading-tight mb-1">${item.name}</h4>
                    <div class="text-neon font-black text-sm mb-2">$${item.price}</div>
                    <div class="flex items-center gap-3">
                        <button class="decrease-qty w-6 h-6 rounded bg-white/10 hover:bg-white/20 flex items-center justify-center text-xs transition-colors" data-id="${item.id}"><i class="fa-solid fa-minus"></i></button>
                        <span class="text-sm font-bold w-4 text-center">${item.quantity}</span>
                        <button class="increase-qty w-6 h-6 rounded bg-white/10 hover:bg-white/20 flex items-center justify-center text-xs transition-colors" data-id="${item.id}"><i class="fa-solid fa-plus"></i></button>
                    </div>
                </div>
            `;
            cartItemsContainer.appendChild(cartRow);
        });

        // Add events to qty buttons
        document.querySelectorAll('.increase-qty').forEach(btn => {
            btn.addEventListener('click', () => updateQuantity(parseInt(btn.dataset.id), 1));
        });
        document.querySelectorAll('.decrease-qty').forEach(btn => {
            btn.addEventListener('click', () => updateQuantity(parseInt(btn.dataset.id), -1));
        });
    }

    // Sidebar Toggles
    function openCart() {
        cartSidebar.classList.add('open');
        cartOverlay.classList.add('open');
        document.body.classList.add('modal-open');
    }
    
    function closeCart() {
        cartSidebar.classList.remove('open');
        cartOverlay.classList.remove('open');
        document.body.classList.remove('modal-open');
    }

    cartBtn.addEventListener('click', openCart);
    closeCartBtn.addEventListener('click', closeCart);
    cartOverlay.addEventListener('click', closeCart);

    // ----------------------------------------------------
    // Waitlist Modal
    // ----------------------------------------------------
    function openWaitlistModal(id, name) {
        modalDropId.value = id;
        modalSneakerName.textContent = name;
        emailInput.value = '';
        waitlistModal.classList.add('open');
        document.body.classList.add('modal-open');
        setTimeout(() => emailInput.focus(), 300);
    }

    function closeWaitlistModal() {
        waitlistModal.classList.remove('open');
        if (!cartSidebar.classList.contains('open') && !checkoutModal.classList.contains('open')) {
            document.body.classList.remove('modal-open');
        }
    }

    modalOverlays.forEach(el => el.addEventListener('click', closeWaitlistModal));

    waitlistForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const dropId = modalDropId.value;
        const email = emailInput.value;
        const submitBtn = waitlistForm.querySelector('button[type="submit"]');
        
        const originalBtnHtml = submitBtn.innerHTML;
        submitBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i>';
        submitBtn.disabled = true;

        try {
            const response = await fetch(`/api/drops/${dropId}/waitlist`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email })
            });

            if (response.ok) {
                closeWaitlistModal();
                showToast("Success", "You've been added to the waitlist.");
            } else {
                alert('An error occurred. Please try again.');
            }
        } catch (error) {
            console.error('Submit error:', error);
            alert('Network error. Please try again.');
        } finally {
            submitBtn.innerHTML = originalBtnHtml;
            submitBtn.disabled = false;
        }
    });

    // ----------------------------------------------------
    // Checkout Modal
    // ----------------------------------------------------
    proceedCheckoutBtn.addEventListener('click', () => {
        closeCart();
        checkoutModal.classList.add('open');
        document.body.classList.add('modal-open');
    });

    function closeCheckoutModal() {
        checkoutModal.classList.remove('open');
        document.body.classList.remove('modal-open');
        checkoutSuccess.classList.remove('show');
    }

    checkoutOverlays.forEach(el => el.addEventListener('click', () => {
        if(checkoutSuccess.classList.contains('show')) {
            // if success is showing, reset cart and reload drops
            cart = [];
            saveCart();
            fetchDrops();
        }
        closeCheckoutModal();
    }));

    checkoutForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        if (cart.length === 0) return;

        const customerName = document.getElementById('checkout-name').value;
        const email = document.getElementById('checkout-email').value;
        const submitBtn = checkoutForm.querySelector('button[type="submit"]');
        
        const originalBtnHtml = submitBtn.innerHTML;
        submitBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> Processing...';
        submitBtn.disabled = true;

        const payload = {
            customerName: customerName,
            email: email,
            items: cart.map(item => ({
                dropId: item.id,
                quantity: item.quantity,
                price: item.price
            }))
        };

        try {
            const response = await fetch(`/api/orders/checkout`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (response.ok) {
                // Show success screen inside modal
                checkoutSuccess.classList.add('show');
            } else {
                alert('Checkout failed! It might be due to insufficient stock.');
            }
        } catch (error) {
            console.error('Submit error:', error);
            alert('Network error during checkout.');
        } finally {
            submitBtn.innerHTML = originalBtnHtml;
            submitBtn.disabled = false;
        }
    });

    // ----------------------------------------------------
    // Toast
    // ----------------------------------------------------
    function showToast(title, message) {
        toast.querySelector('h4').textContent = title;
        toast.querySelector('p').textContent = message;
        toast.classList.remove('translate-y-20', 'opacity-0');
        setTimeout(() => toast.classList.add('translate-y-20', 'opacity-0'), 3000);
    }
});
