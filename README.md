# Event-raising collections
This library introduces a wrapper for `ICollection<T>` (and implementations for `IList<T>` & `ISet<T>`) which raises events when its state is modified.

This was first created for helping with entity-based projects which use [NHibernate].
In the NHibernate landscape, when using the `inverse` collection mapping, it is important to back-fill child items' references to their parents, upon adding them to collections.
Thus, as well as performing `parent.Children.Add(child)`, you must also set `child.Parent = parent`.
This library accomplishes that by allowing the parent (which exposes that collection of its children) to define that after adding an item, that item is manipulated automatically, doing the back-filling of data.

[NHibernate]: http://nhibernate.info/

## Entity-based demonstration
Here is a quick demonstration of some sample code which accomplishes the above use-case:

```csharp
public class Parent
{
  private EventRaisingSetWrapper<Child> _children;

  // This is the property exposed to the rest of your API, which is a wrapped collection instance
  // NHibernate mappings would not be bound to this property.
  public virtual ISet<Child> Children { get { return _children.Collection; } }

  // This property is bound to with NHibernate (it can bind to protected members), and it exposes the original,
  // unwrapped collection instance (with no events)/
  protected virtual ISet<Child> ChildrenSource {
    get { return _children.SourceCollection; }
    set { _children.SourceCollection = value; }
  }

  // In the constructor, instantiate a wrapper instance and then set up its after add/remove event handlers
  public Parent()
  {
    _children = new EventRaisingSetWrapper<Child>(new HashSet<Child>());
    _children.AfterAdd += (sender, e) => e.Item.Parent = this;
    _children.AfterRemove += (sender, e) => e.Item.Parent = null;
  }
}

public class Child
{
  public virtual Parent Parent
  {
    get;
    set;
  }
}
```

## Cancelling modification
The **before** events all have a `Cancel()` method.  This method, if called in a handler, will cancel the modification (addition to/removal from) the collection.
Use this with care, as it can cause some very confusing scenarios if other client code expects the addition/removal to have been a success.

## Open source license
All source files within this project are released as open source software, under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
