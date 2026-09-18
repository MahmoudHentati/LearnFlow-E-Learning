"""
Base Page - Page Object Model
LearnFlow E-Training Platform

Classe de base pour tous les Page Objects.
Fournit les méthodes communes de navigation et d'interaction.

Généré avec l'assistance de Claude (Anthropic)
"""

from playwright.sync_api import Page, expect
from typing import Optional


class BasePage:
    """Classe de base pour le Page Object Model (POM)"""
    
    # URL de base de l'application LearnFlow
    BASE_URL = "https://localhost:7001"  # Ajuster selon votre configuration
    
    def __init__(self, page: Page):
        """
        Initialise la page de base.
        
        Args:
            page: Instance de Page Playwright
        """
        self.page = page
    
    def navigate_to(self, path: str = "") -> None:
        """
        Navigue vers une URL spécifique.
        
        Args:
            path: Chemin relatif à ajouter à l'URL de base
        """
        url = f"{self.BASE_URL}/{path}"
        self.page.goto(url)
    
    def get_title(self) -> str:
        """Retourne le titre de la page actuelle."""
        return self.page.title()
    
    def get_url(self) -> str:
        """Retourne l'URL actuelle."""
        return self.page.url
    
    def wait_for_load(self, timeout: int = 30000) -> None:
        """
        Attend que la page soit complètement chargée.
        
        Args:
            timeout: Timeout en millisecondes
        """
        self.page.wait_for_load_state("networkidle", timeout=timeout)
    
    def click_element(self, selector: str) -> None:
        """
        Clique sur un élément.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
        """
        self.page.click(selector)
    
    def fill_input(self, selector: str, value: str) -> None:
        """
        Remplit un champ de saisie.
        
        Args:
            selector: Sélecteur CSS ou Playwright du champ
            value: Valeur à saisir
        """
        self.page.fill(selector, value)
    
    def get_text(self, selector: str) -> str:
        """
        Récupère le texte d'un élément.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
            
        Returns:
            Le texte contenu dans l'élément
        """
        return self.page.text_content(selector) or ""
    
    def is_visible(self, selector: str) -> bool:
        """
        Vérifie si un élément est visible.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
            
        Returns:
            True si l'élément est visible, False sinon
        """
        return self.page.is_visible(selector)
    
    def wait_for_element(self, selector: str, timeout: int = 10000) -> None:
        """
        Attend qu'un élément soit visible.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
            timeout: Timeout en millisecondes
        """
        self.page.wait_for_selector(selector, timeout=timeout)
    
    def take_screenshot(self, name: str) -> str:
        """
        Prend une capture d'écran.
        
        Args:
            name: Nom du fichier de capture
            
        Returns:
            Chemin du fichier de capture
        """
        path = f"rapport_execution/screenshots/{name}.png"
        self.page.screenshot(path=path)
        return path
    
    def expect_url_contains(self, text: str) -> None:
        """
        Vérifie que l'URL contient un texte spécifique.
        
        Args:
            text: Texte attendu dans l'URL
        """
        expect(self.page).to_have_url(f"*{text}*")
    
    def expect_element_visible(self, selector: str) -> None:
        """
        Vérifie qu'un élément est visible.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
        """
        expect(self.page.locator(selector)).to_be_visible()
    
    def expect_text_present(self, selector: str, text: str) -> None:
        """
        Vérifie qu'un élément contient un texte spécifique.
        
        Args:
            selector: Sélecteur CSS ou Playwright de l'élément
            text: Texte attendu
        """
        expect(self.page.locator(selector)).to_contain_text(text)
