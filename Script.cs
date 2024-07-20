using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Script
{
    static void Main(string[] args)
    {
        using var driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://store.steampowered.com/charts/topselling/RU");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElements(By.CssSelector("div._1n_4-zvf0n4aqGEksbgW9N")).Count > 0);
        var gameTables = driver.FindElements(By.CssSelector("div._3sJkwsBQuiAc_i3VOWX4tv table tbody tr"));
        Console.WriteLine("{0,-10} {1,-40} {2,-10}", "Номер", "Название игры", "Стоимость");
        for (int i = 0; i < Math.Min(10, gameTables.Count); i++)
        {
            var gameTable = gameTables[i];
            int rank = i + 1;
            var nameElement = gameTable.FindElement(By.CssSelector("td._18kGHKeOavDDdJVs9FVhpo a div._1n_4-zvf0n4aqGEksbgW9N"));
            string gameName = nameElement.Text.Trim();
            string gamePrice = "";
            try
            {
                var priceElement = gameTable.FindElement(By.CssSelector("td._3IyfUchPbsYMEaGjJU3GOP div div div"));
                gamePrice = priceElement.Text.Trim();
                if (gamePrice.Contains("Бесплатно") || gamePrice.Contains("руб"))
                {

                }
                else
                {
                    var altPriceElement = gameTable.FindElement(By.CssSelector("td._3IyfUchPbsYMEaGjJU3GOP > div > div > div._3NhLu7mTdty7JufpSpz6Re > div._3j4dI1yA7cRfCvK8h406OB"));
                    gamePrice = altPriceElement.Text.Trim();
                }
            }
            catch (NoSuchElementException)
            {
                 var altPriceElement = gameTable.FindElement(By.CssSelector("td._3IyfUchPbsYMEaGjJU3GOP > div > div > div._3NhLu7mTdty7JufpSpz6Re > div._3j4dI1yA7cRfCvK8h406OB"));
                gamePrice = altPriceElement.Text.Trim();
            }
            Console.WriteLine("{0,-10} {1,-40} {2,-10}", rank, gameName, gamePrice);
        }
        driver.Quit();
    }
}
