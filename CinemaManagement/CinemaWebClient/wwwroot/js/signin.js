function onSignIn(googleUser) {
	var id_token = googleUser.credential;
	console.log(id_token)
	window.location.href = "/SignIn?handler=SignInGoogle&idToken=" + id_token
};
window.onload = function () {
	google.accounts.id.initialize({
		client_id: '965301919060-vi54tl3925kjdhprfvusnoshavhhn31k.apps.googleusercontent.com',
		callback: onSignIn
	});
	google.accounts.id.prompt((notification) => {
		if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
		}
	});
};
function handleCredentialResponse(response) {
	onSignIn(response);
}