// Wait for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", function () {

    // Mobile navigation (hamburger under 1000px)
    const navToggle = document.querySelector(".nav-toggle");
    const primaryNav = document.getElementById("primary-nav");

    function closePrimaryNav() {
        if (!navToggle || !primaryNav) return;
        primaryNav.classList.remove("is-open");
        navToggle.setAttribute("aria-expanded", "false");
    }

    function togglePrimaryNav() {
        if (!navToggle || !primaryNav) return;
        const isOpen = primaryNav.classList.toggle("is-open");
        navToggle.setAttribute("aria-expanded", isOpen ? "true" : "false");
    }

    if (navToggle && primaryNav) {
        navToggle.addEventListener("click", function (e) {
            e.preventDefault();
            togglePrimaryNav();
        });

        document.addEventListener("click", function (e) {
            if (!primaryNav.classList.contains("is-open")) return;
            const target = e.target;
            if (!(target instanceof Node)) return;

            if (navToggle.contains(target) || primaryNav.contains(target)) return;
            closePrimaryNav();
        });

        document.addEventListener("keydown", function (e) {
            if (e.key === "Escape") closePrimaryNav();
        });

        window.addEventListener("resize", function () {
            // If we leave the mobile breakpoint, ensure nav is reset
            if (window.innerWidth > 1000) {
                closePrimaryNav();
            }
        });
    }

    // Language selector dropdown
    const langForm = document.querySelector('[data-lang-selector]');
    const langTrigger = langForm ? langForm.querySelector('.lang-trigger') : null;
    const langMenu = langForm ? langForm.querySelector('.lang-menu') : null;

    function closeLangMenu() {
        if (!langTrigger || !langMenu) return;
        langMenu.hidden = true;
        langTrigger.setAttribute('aria-expanded', 'false');
    }

    function toggleLangMenu() {
        if (!langTrigger || !langMenu) return;
        const isOpen = !langMenu.hidden;
        langMenu.hidden = isOpen;
        langTrigger.setAttribute('aria-expanded', isOpen ? 'false' : 'true');
    }

    if (langTrigger && langMenu) {
        closeLangMenu();

        langTrigger.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            toggleLangMenu();
        });

        document.addEventListener('click', function (e) {
            if (langMenu.hidden) return;
            const target = e.target;
            if (!(target instanceof Node)) return;
            if (langForm && langForm.contains(target)) return;
            closeLangMenu();
        });

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') closeLangMenu();
        });

        window.addEventListener('resize', function () {
            closeLangMenu();
        });
    }

       const modalOverlay = document.getElementById("modal-overlay");
    const modalContainer = document.getElementById("modal-container");
    const modalCloseBtn = document.querySelector(".modal-close-btn");
     const modalFullscreenToggle = document.querySelector(".modal-fullscreen-toggle");
    const modalFullscreenIcon = document.querySelector(".modal-fullscreen-icon");
    let currentExerciseId = null; // Track the current exercise URL
     let isExerciseModal = false;

    const maximizeIconSrc = "/images/maximize.png";
    const minimizeIconSrc = "/images/minimize.png";
    
    // Find all triggers
    const triggers = document.querySelectorAll(".modal-trigger");

    triggers.forEach(trigger => {
        trigger.addEventListener("click", function (e) {
            e.preventDefault(); // Stop the link from navigating

            const type = this.dataset.type;
            
            if (type === "video") {
                const videoId = this.dataset.id;
                openVideoModal(videoId);
            } else if (type === "exercise") {
                const exerciseId = this.dataset.id;
                openExerciseModal(exerciseId);
            }
        });
    });

    function openVideoModal(videoId) {
        isExerciseModal = false;

        if (modalOverlay) {
            modalOverlay.classList.add("is-video");
        }

        if (modalFullscreenToggle) {
            modalFullscreenToggle.hidden = true;
            modalFullscreenToggle.setAttribute("aria-label", "Expand to fullscreen");
        }
        if (modalFullscreenIcon) {
            modalFullscreenIcon.src = maximizeIconSrc;
        }
        if (modalOverlay) {
            modalOverlay.classList.remove("is-fullscreen");
        }

        // Create YouTube embed iframe
        modalContainer.innerHTML = `<iframe 
            width="560" 
            height="315" 
            src="https://www.youtube.com/embed/${videoId}?autoplay=1" 
            title="YouTube video player" 
            frameborder="0" 
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
            allowfullscreen></iframe>`;
        
        showModal();
    }

    function openExerciseModal(exerciseId) {
        // Create exercise URL from exerciseId
        currentExerciseId= exerciseId;
        isExerciseModal = true;

        if (modalOverlay) {
            modalOverlay.classList.remove("is-video");
        }

        if (modalFullscreenToggle) {
            modalFullscreenToggle.hidden = false;
            modalFullscreenToggle.setAttribute("aria-label", "Expand to fullscreen");
        }
        if (modalFullscreenIcon) {
            modalFullscreenIcon.src = maximizeIconSrc;
        }
        if (modalOverlay) {
            modalOverlay.classList.remove("is-fullscreen");
        }

        const currentExerciseUrl = `https://tartalom.okosdoboz.hu/blobstorage/Uploads/Packages/Package-${exerciseId}/index.html?lang=hu_hu&mod=gyakorlas&disablelangbuttons=1`;
        
        // Create exercise iframe
        modalContainer.innerHTML = `<iframe 
            src="${currentExerciseUrl}" 
            title="Exercise Content"></iframe>`;
            
        showModal();
    }

    function showModal() {
        modalOverlay.style.display = "flex"; // Show the modal
    }

    function closeModal() {
        modalOverlay.style.display = "none"; // Hide the modal
        modalContainer.innerHTML = ""; // Clear the content
        currentExerciseId = null; // Clear the Id

        isExerciseModal = false;
        if (modalFullscreenToggle) {
            modalFullscreenToggle.hidden = true;
            modalFullscreenToggle.setAttribute("aria-label", "Expand to fullscreen");
        }
        if (modalFullscreenIcon) {
            modalFullscreenIcon.src = maximizeIconSrc;
        }
        if (modalOverlay) {
            modalOverlay.classList.remove("is-video");
            modalOverlay.classList.remove("is-fullscreen");
        }
    }

    if (modalFullscreenToggle && modalOverlay) {
        modalFullscreenToggle.addEventListener("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            if (!isExerciseModal) return;

            const isFullscreen = modalOverlay.classList.toggle("is-fullscreen");
            modalFullscreenToggle.setAttribute(
                "aria-label",
                isFullscreen ? "Collapse to original size" : "Expand to fullscreen"
            );

            if (modalFullscreenIcon) {
                modalFullscreenIcon.src = isFullscreen ? minimizeIconSrc : maximizeIconSrc;
            }
        });
    }

    // Close modal when clicking the 'x' button or the overlay
    modalCloseBtn.addEventListener("click", closeModal);
    modalOverlay.addEventListener("click", function (e) {
        // Only close if the click is on the overlay itself, not the content
        if (e.target === modalOverlay) {
            closeModal();
        }
    });

    // Listen for messages from iframes (exercises) and log scores
    window.addEventListener('message', function (event) {
        try {

            // Verify the sender's origin
            //if (event.origin !== "https://trusted-domain.com") return;
            
            var data = event.data;
            if (data && data.type === 'gyakorloEredmeny') {
                // Log the received score to the console
                console.log('Received exercise score:', data.scorepercent, 'Id:', currentExerciseId);

                if (!currentExerciseId) return;

                const score = Number.parseInt(data.scorepercent, 10);
                if (!Number.isFinite(score) || score < 0) return;

                fetch('/api/results', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        exerciseId: String(currentExerciseId),
                        score: score
                    })
                }).catch(function (err) {
                    console.warn('Failed to save exercise result:', err);
                });
            }
        } catch (err) {
            console.warn('Error handling postMessage from iframe:', err);
        }
    }, false);
});

// Slideshow functionality
let currentSlideIndex = 1;

function changeSlide(n) {
    showSlide(currentSlideIndex += n);
}

function currentSlide(n) {
    showSlide(currentSlideIndex = n);
}

function showSlide(n) {
    const slides = document.querySelectorAll(".slide");
    const dots = document.querySelectorAll(".dot");
    
    if (n > slides.length) {
        currentSlideIndex = 1;
    }
    if (n < 1) {
        currentSlideIndex = slides.length;
    }
    
    slides.forEach(slide => slide.classList.remove("active"));
    dots.forEach(dot => dot.classList.remove("active"));
    
    if (slides[currentSlideIndex - 1]) {
        slides[currentSlideIndex - 1].classList.add("active");
    }
    if (dots[currentSlideIndex - 1]) {
        dots[currentSlideIndex - 1].classList.add("active");
    }
}

// Initialize slideshow on page load
document.addEventListener("DOMContentLoaded", function () {
    const slides = document.querySelectorAll(".slide");
    if (slides.length > 0) {
        showSlide(currentSlideIndex);
    }
});

/* --- Smooth Scroll to Section for Sidebar Navigation --- */
function scrollToSection(sectionId) {
    const section = document.getElementById(sectionId);
    if (section) {
        // Scroll to the section with a small offset from the top
        const yOffset = -100; // Adjust this offset as needed
        const y = section.getBoundingClientRect().top + window.pageYOffset + yOffset;
        window.scrollTo({ top: y, behavior: 'smooth' });
    }
}

// Make function globally available
window.scrollToSection = scrollToSection;
