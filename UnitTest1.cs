namespace revdotcom_pw_tests;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{

    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.SetViewportSizeAsync(1600, 1200);
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        // 
        await Page.GotoAsync("https://test.rev.ai");

        var signUpAnchor = Page.Locator("id=account-signup-menu");

        await signUpAnchor.ClickAsync();

        await Expect(Page).ToHaveTitleAsync("Free Trial Sign Up | Rev AI");

        // Fill in the Sign Up fields
        var firstNameInput = Page.Locator("id=signup-firstname");
        var firstName = GetFixedLengthString(10);
        await firstNameInput.TypeAsync(firstName);
        await Expect(firstNameInput).ToHaveValueAsync(firstName);

        var lastNameInput = Page.Locator("id=signup-lastname");
        var lastName = GetFixedLengthString(10);
        await lastNameInput.TypeAsync(lastName);
        await Expect(lastNameInput).ToHaveValueAsync(lastName);

        var emailInput = Page.Locator("id=signup-email");
        var emailAddress = "kevin.gann+" + GetFixedLengthString(5) + "@rev.com";
        await emailInput.TypeAsync(emailAddress);
        await Expect(emailInput).ToHaveValueAsync(emailAddress);

        var passwordInput = Page.Locator("id=signup-password");
        var password = GetFixedLengthString(9);
        await passwordInput.TypeAsync(password);
        await Expect(passwordInput).ToHaveValueAsync(password);

        var retypePasswordInput = Page.Locator("id=signup-confirm-password");
        await retypePasswordInput.TypeAsync(password);
        await Expect(retypePasswordInput).ToHaveValueAsync(password);

        var signUpButton = Page.Locator("id=signup-submit-button");
        await Expect(signUpButton).ToBeEnabledAsync();
        await signUpButton.ClickAsync(new Microsoft.Playwright.LocatorClickOptions { Delay=100});
        await Expect(signUpButton).ToBeHiddenAsync();

        // The form has been filled out.
        var resendEmailButton = Page.Locator("id=resend-verification-email-btn");
        await Expect(resendEmailButton).ToBeVisibleAsync();

        // Click the displayed Login link.
        var loginAnchor = Page.Locator("id=signup-page-login-btn");
        await loginAnchor.ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Identity Service");
    }

    private static string GetFixedLengthString(int len)
    {
        const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        StringBuilder sb = new StringBuilder();
        Random randomNumber = new Random();
        for (int i = 0; i < len; i++)
        {
            sb.Append(chars[randomNumber.Next(0, chars.Length)]);
        }
        return sb.ToString();
    }


}