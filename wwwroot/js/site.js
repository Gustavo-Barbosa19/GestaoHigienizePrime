(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initSidebar();
        initToastify();
        initGlobalSearch();
        initMasks();
        initValidation();
        initLoading();
    });

    function initSidebar() {
        const sidebar = document.getElementById('sidebar');
        const wrapper = document.getElementById('wrapper');

        if (sidebar && wrapper) {
            const toggle = document.querySelector('[onclick="toggleSidebar()"]');
            if (toggle) {
                toggle.addEventListener('click', function (e) {
                    e.preventDefault();
                    wrapper.classList.toggle('toggled');
                });
            }
        }
    }

    function initToastify() {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(function (alert) {
            setTimeout(function () {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000);
        });
    }

    function initGlobalSearch() {
        const searchInput = document.getElementById('searchInput');
        if (!searchInput) return;

        let debounceTimer;

        searchInput.addEventListener('input', function () {
            clearTimeout(debounceTimer);
            const term = this.value.trim();

            if (term.length < 2) {
                removeSearchResults();
                return;
            }

            debounceTimer = setTimeout(function () {
                performSearch(term);
            }, 400);
        });

        document.addEventListener('click', function (e) {
            if (!e.target.closest('#globalSearch')) {
                removeSearchResults();
            }
        });
    }

    function performSearch(term) {
        const searchContainer = document.getElementById('globalSearch');
        let resultsDiv = document.querySelector('.search-results');

        if (!resultsDiv) {
            resultsDiv = document.createElement('div');
            resultsDiv.className = 'search-results';
            searchContainer.style.position = 'relative';
            searchContainer.appendChild(resultsDiv);
        }

        resultsDiv.innerHTML = '<div class="text-center py-3"><div class="spinner" style="width:24px;height:24px;margin:0 auto;"></div></div>';
        resultsDiv.classList.add('show');

        const searchUrl = window.location.origin + '/Clientes?searchTerm=' + encodeURIComponent(term);
        window.location.href = searchUrl;
    }

    function removeSearchResults() {
        const results = document.querySelector('.search-results');
        if (results) {
            results.classList.remove('show');
        }
    }

    function initMasks() {
        document.querySelectorAll('.phone-mask').forEach(function (input) {
            input.addEventListener('input', function () {
                let value = this.value.replace(/\D/g, '');
                if (value.length <= 10) {
                    value = value.replace(/^(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
                } else {
                    value = value.replace(/^(\d{2})(\d{5})(\d{0,4})/, '($1) $2-$3');
                }
                this.value = value;
            });
        });

        document.querySelectorAll('.cep-mask').forEach(function (input) {
            input.addEventListener('input', function () {
                let value = this.value.replace(/\D/g, '');
                value = value.replace(/^(\d{5})(\d{0,3})/, '$1-$2');
                this.value = value;
            });
        });
    }

    function initValidation() {
        const forms = document.querySelectorAll('.needs-validation');
        forms.forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }

    function initLoading() {
        const forms = document.querySelectorAll('form');
        forms.forEach(function (form) {
            form.addEventListener('submit', function () {
                const submitBtn = form.querySelector('button[type="submit"]');
                if (submitBtn && form.checkValidity()) {
                    submitBtn.disabled = true;
                    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Aguarde...';
                }
            });
        });
    }

    window.toggleSidebar = function () {
        const wrapper = document.getElementById('wrapper');
        if (wrapper) {
            wrapper.classList.toggle('toggled');
        }
    };

    window.showLoading = function () {
        let overlay = document.querySelector('.loading-overlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.className = 'loading-overlay';
            overlay.innerHTML = '<div class="spinner"></div>';
            document.body.appendChild(overlay);
        }
        overlay.classList.add('show');
    };

    window.hideLoading = function () {
        const overlay = document.querySelector('.loading-overlay');
        if (overlay) {
            overlay.classList.remove('show');
        }
    };
})();
