document.addEventListener('DOMContentLoaded', () => {
    
    const loginView = document.getElementById('login-view');
    const dashboardView = document.getElementById('dashboard-view');
    const loginForm = document.getElementById('admin-login-form');
    const logoutBtn = document.getElementById('logout-btn');

    // Dashboard Elements
    const statRevenue = document.getElementById('stat-revenue');
    const statOrders = document.getElementById('stat-orders');
    const statItems = document.getElementById('stat-items');
    const topSellersBody = document.getElementById('top-sellers-body');
    const lowStockList = document.getElementById('low-stock-list');

    // Check auth on load
    checkAuth();

    function checkAuth() {
        const token = localStorage.getItem('hype_admin_token');
        if (token) {
            showDashboard();
            fetchDashboardData(token);
        } else {
            showLogin();
        }
    }

    function showLogin() {
        loginView.classList.remove('hidden');
        dashboardView.classList.add('hidden');
    }

    function showDashboard() {
        loginView.classList.add('hidden');
        dashboardView.classList.remove('hidden');
    }

    // Login Logic
    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;
        const submitBtn = loginForm.querySelector('button[type="submit"]');
        
        submitBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i>';
        
        try {
            const response = await fetch('/api/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('hype_admin_token', data.token);
                checkAuth();
            } else {
                alert('Invalid credentials!');
            }
        } catch (error) {
            console.error('Login error', error);
            alert('Error connecting to server.');
        } finally {
            submitBtn.innerHTML = 'Login to System';
        }
    });

    // Logout Logic
    logoutBtn.addEventListener('click', () => {
        localStorage.removeItem('hype_admin_token');
        checkAuth();
    });

    // Fetch Dashboard Data
    async function fetchDashboardData(token) {
        try {
            const response = await fetch('/api/reports/dashboard', {
                headers: {
                    'Authorization': 'Bearer ' + token
                }
            });

            if (response.status === 401) {
                localStorage.removeItem('hype_admin_token');
                checkAuth();
                return;
            }

            if (response.ok) {
                const stats = await response.json();
                renderDashboard(stats);
            }
        } catch (error) {
            console.error('Dashboard fetch error', error);
        }
    }

    function renderDashboard(stats) {
        // Stats
        statRevenue.textContent = '$' + (stats.totalRevenue ? stats.totalRevenue.toFixed(2) : '0.00');
        statOrders.textContent = stats.totalOrders;
        statItems.textContent = stats.totalItemsSold;

        // Top Sellers
        if (stats.topSellers && stats.topSellers.length > 0) {
            topSellersBody.innerHTML = stats.topSellers.map((item, index) => `
                <tr class="border-b border-white/5 hover:bg-white/5 transition-colors">
                    <td class="py-4 flex items-center">
                        <span class="w-6 text-gray-500 font-bold mr-2">#${index + 1}</span>
                        <span class="font-semibold text-white">${item.sneakerName}</span>
                    </td>
                    <td class="py-4 text-right text-neon font-bold">${item.totalQuantitySold}</td>
                </tr>
            `).join('');
        } else {
            topSellersBody.innerHTML = `<tr><td colspan="2" class="py-6 text-center text-gray-500">No sales data yet.</td></tr>`;
        }

        // Low Stock
        if (stats.lowStockAlerts && stats.lowStockAlerts.length > 0) {
            lowStockList.innerHTML = stats.lowStockAlerts.map(item => {
                const stockColor = item.quantity === 0 ? 'text-red-500' : 'text-orange-500';
                return `
                <li class="flex justify-between items-center bg-[#111] p-4 rounded-xl border border-white/5">
                    <div>
                        <div class="font-bold text-sm text-white">${item.sneakerName}</div>
                        <div class="text-xs text-gray-400">Size: ${item.size}</div>
                    </div>
                    <div class="font-black ${stockColor}">${item.quantity} Left</div>
                </li>
            `}).join('');
        } else {
            lowStockList.innerHTML = `<li class="text-center py-4 text-gray-500">Stock levels look good!</li>`;
        }
    }
});
